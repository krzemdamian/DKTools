#Created by Damian Krzemiński, contact: krzemodam2@gmail.com
	
def detect_face_reference(el, projection, ray_offset=0.00260416666):
	"""Function returns detected face reference or None if not found 
	and proximate distance from element location point."""
	
	hit = projection.FindNearest(*ray_conf(el, ray_offset))
	#self.log("Found reference for selection: {}".format(hit))
	
	if hit:
		reference = hit.GetReference()
			
		return reference, (hit.Proximity - ray_offset)
	else:
		return None, None


	
def clone_element_on_face(el,reference):
	"""
	TRANSACTION MUST BE STARTED!
	Function atempts to clone  passed element on provided face reference.
	It copies element in original element location and copies editable parameters' 
	values.
	"""
	try:
		# activate symbol, this metod is to ensure that element symbol is visible for API (?)
		# its better to have that method executed, google for details
		el.Symbol.Activate()
		
		# new element created, this is only done when transaction is already opened
		element_clone = cdoc.NewFamilyInstance.Overloads[Reference,XYZ,XYZ,FamilySymbol](reference, el.Location.Point, el.HandOrientation, el.Symbol)
		
		
		# this code copies parameters
		old_params_dict = {old_param.Definition.Name: old_param for old_param in el.Parameters}
		
		new_params_dict = {new_param.Definition.Name: new_param for new_param in element_clone.Parameters}
		
		for key, new_p in new_params_dict.iteritems():
			if not new_p.IsReadOnly:
				if new_p.StorageType == Autodesk.Revit.DB.StorageType.Integer:
					new_p.Set(old_params_dict[key].AsInteger())

				elif new_p.StorageType == Autodesk.Revit.DB.StorageType.Double:
					new_p.Set(old_params_dict[key].AsDouble())

				elif new_p.StorageType == Autodesk.Revit.DB.StorageType.String:
					new_p.Set.Overloads[str](old_params_dict[key].AsString())

				elif new_p.StorageType == Autodesk.Revit.DB.StorageType.ElementId:
					new_p.Set(old_params_dict[key].AsElementId())

		return element_clone
	
	except Exception as exception:
		# exeption name
		f.log("\r\n\r\n")
		f.log(type(exception).__name__)
		f.log(str(exception))
		return None

		
	
def ray_conf(el, ray_offset = 0.00260416666):
	"""Method returns tuple (ray_origin, ray_direction). First object 
	in tuple is XYZ object representing origin of ray intersector retracted
	from orgin of given element by given tolerance given in feet, second 
	element is XYZ object representing ray direction."""
	
	if el:
		# calculating direction
		el_facing_orientation = el.FacingOrientation
		el_hand_oriantation = el.HandOrientation
		ray_direction = el.FacingOrientation.CrossProduct(el.HandOrientation)
		if el.FacingFlipped == True:
			ray_direction = ray_direction.Negate()
		
		# calculating point
		el_location  = el.Location.Point
		origin_shift = ray_direction.Multiply(0-ray_offset)
		ray_origin = el_location.Add(origin_shift)

		return ray_origin, ray_direction
	
	
	
def filter_columns_and_beams():
	"""Function returning filter passing elements which can be hosts for strucutral elements"""
	
	
	# quick filter for multiple categories
	cats = List[BuiltInCategory]()
	cats.Add(BuiltInCategory.OST_StructuralColumns)
	cats.Add(BuiltInCategory.OST_StructuralFraming)
	qfilter = ElementMulticategoryFilter(cats)
	
	
	# slow filter passing elements that are not Joists
	sfilter = StructuralInstanceUsageFilter(StructuralInstanceUsage.Joist, True)
	
	# combine filters
	cfilter = LogicalAndFilter(qfilter, sfilter)
	
	return cfilter
	
	
		

def create_3d_view():
	"""Create 3D view"""
	
	# only to get proper 3d view type
	viewFamilyTypes = FilteredElementCollector(doc).OfClass(ViewFamilyType)
	view_type_3d = [viewFamilyType for viewFamilyType in viewFamilyTypes if viewFamilyType.ViewFamily == ViewFamily.ThreeDimensional]
	view_type_3d = view_type_3d[0]
	
	### DEBUGING EXAMPLE: ###	
	# not needed anymore, this filter only slowes down 3d view creation
	#filtered_elements = FilteredElementCollector(doc).WherePasses(self.filter_columns_and_beams()).WhereElementIsNotElementType().ToElementIds()
	
	#t = Transaction(doc, "Create 3D View")
	#t.Start()
	#self.log(view_type_3d)
	view_3d = View3D.CreateIsometric(doc, view_type_3d.Id)
	#view_3d.Name = "DKTools Host Verification View"
	view_3d.DetailLevel = ViewDetailLevel.Fine
	view_3d.AreAnalyticalModelCategoriesHidden = True
	view_3d.AreAnnotationCategoriesHidden = True
	view_3d.AreImportCategoriesHidden = True
	view_3d.ArePointCloudsHidden = True
	view_3d.DisplayStyle = DisplayStyle.FlatColors
	# turned off for performance testing:
	#view_3d.IsolateElementsTemporary(filtered_elements)
	#view_3d.ConvertTemporaryHideIsolateToPermanent()
	#t.Commit()
	return view_3d



def verify_reference(el, filter_columns_and_beams):
	''' 
	Function returns distance between element insertion point and point projected on face form elements reference. Returns infinity if projected point outside face plane.
	'''
	
	if type(el) == FamilyInstance:
		if el.HostFace:
			face_ref = el.HostFace  # reference
			el_from_ref = doc.GetElement(face_ref) # element from reference
			face = el_from_ref.GetGeometryObjectFromReference(face_ref) # face object
			el_location_point = el.Location.Point # element location point
					
			transformation = el_from_ref.GetTransform().Inverse
			transformed_location_point = transformation.OfPoint(el_location_point)
			
			#f.log("THIS IS DEBUG")
			#f.log("El location point: {}, Face origin: {}".format(transformed_location_point, face.Origin))
			projected_point = face.Project(transformed_location_point)
			if projected_point:
				return projected_point.Distance
			else:
				solid_face = face_full_shape(face)
				projected_point2 = solid_face.Project(transformed_location_point)
				if projected_point2:
					return projected_point2.Distance
				else:
					return float("inf")
				
		else:
			return float("inf")



def face_reference_by_boundling_box(el, beam_and_column_filter, reference=True):
	'''
	Function returns face reference. It searches beam and column faces which normal are parallel to strucutral connection host plane. It returns the closest face reference to element's location point. This founction should be used in case ray face detection doesn't finds reference.
	'''
	
	#default return
	result = None, None
	
	# element location point
	location_point = el.Location.Point
	
	# create filter for colector (beams not beeing joist, columns in boundling box)
	boundling_box_filter = BoundingBoxContainsPointFilter(location_point)
	filter = LogicalAndFilter(beam_and_column_filter,boundling_box_filter)
	
	# sieve potential hosts (beams not beeing joists, columns) in boundling box 
	filtered_elements = FilteredElementCollector(doc).WherePasses(filter).WhereElementIsNotElementType().ToElements()
	
	
	### EXAMPLE OF DEBUGING: ###
	#f.log(filtered_elements)
	# test print
	#f.log("elements in collector")
	#f.log(filtered_elements.Count)
	
	
	# calculate element hosting surface normal
	el_facing_orientation = el.FacingOrientation
	el_hand_oriantation = el.HandOrientation
	element_normal = el.FacingOrientation.CrossProduct(el.HandOrientation)
	if el.FacingFlipped == True:
		element_normal = element_normal.Negate()
		
	#f.log(element_normal)

	# Get faces from elements in boundling box:
	for f_el in filtered_elements:
		
		# options for geometry retrieval
		opts = Options()
		opts.DetailLevel = ViewDetailLevel.Fine
		opts.ComputeReferences = True
		
		# instantiate potential element geometry
		geomet = f_el.get_Geometry(opts)
		#f.log(geomet)

		transformation = f_el.GetTransform().Inverse
		
		transformed_location_point = transformation.OfPoint(location_point)
		#f.log(transformed_location_point)
		#f.log("This is location point of potential hosting element: {}".format(f_el_location))
		
		# retrive geomtery elements from geometry instance
		for geom in geomet:
			# solid elements of geometry instance (deep nesting)
			gs = geom.GetSymbolGeometry()
			
			for solid in gs:
				# sieve empty solids
				if solid.SurfaceArea:
					face_array = solid.Faces
					# get and sieve parallel faces
					for face in face_array:
						face_normal = face.FaceNormal
						normals_dot_product = round(abs(face_normal.DotProduct(element_normal)),3)
						
						if normals_dot_product == 1:
							
							element_to_face_distance = transformed_location_point.DistanceTo(face_center(face))
							
							face_full_shape(face)

							#f.log("Distance to element after translation: {}".format(element_to_face_distance))
							
							if not 'smallest_distance' in locals():
								smallest_distance = element_to_face_distance
								if reference:
									result = face.Reference, smallest_distance
								else:
									result = face, smallest_distance
								
							if element_to_face_distance < smallest_distance:
								smallest_distance = element_to_face_distance
								if reference:
									result = face.Reference, smallest_distance
								else:
									result = face, smallest_distance			
								
	return result

def face_center(face):
	
	vertices = face.Triangulate().Vertices
	max = vertices[0]
	min = vertices[0]
	
	for vx in vertices:
		if vx.X>max.X: max = XYZ (vx.X,max.Y,max.Z)
		if vx.Y>max.Y: max = XYZ (max.X,vx.Y,max.Z)
		if vx.Z>max.Z: max = XYZ (max.X,max.Y,vx.Z)
		if vx.X<min.X: min = XYZ (vx.X,min.Y,min.Z)
		if vx.Y<min.Y: min = XYZ (min.X,vx.Y,min.Z)
		if vx.Z<min.Z: min = XYZ (min.X,min.Y,vx.Z)
		
	mid_sum = max.Add(min)
	
	mid = XYZ(mid_sum.X/2,mid_sum.Y/2,mid_sum.Z/2)
	
	return mid
	
	
def face_full_shape(face):
	'''
	Function returns face with full area created by most external edge of parsed face. 
	'''
	
	try:
		all_curves = face.GetEdgesAsCurveLoops()
		
		for curve in all_curves:
			curve_length = curve.GetExactLength()

			if not 'longest_length' in locals():
				longest_length = curve_length
				longest_curve = curve
			
			if curve_length > longest_length:
				longest_length = curve_length
		
		curveList = List[CurveLoop]()
		curveList.Add(longest_curve)

		temporary_solid_geometry = GeometryCreationUtilities.CreateExtrusionGeometry(curveList, face.FaceNormal, 1)

		temp_solid_face = temporary_solid_geometry.Faces[1]

		return temp_solid_face
		
	except Exception as exception:
		f.log(type(exception).__name__)
		f.log(str(exception))
		return None
		
		
### END OF FUNCTION DEFINITIONS  ###





### START OF PROGRAM LOGIC ###

f.ClientSize = System.Drawing.Size(750, 300)
#f.log('external event script started')
f.TopMost = True
f.Refresh()

# get selection of elements
sElIds = uidoc.Selection.GetElementIds()


# fiter strucutral connections
filtered_elements = FilteredElementCollector(doc, sElIds).OfCategory(BuiltInCategory.OST_StructConnections).WhereElementIsNotElementType().ToElements()


# count elements and print info
count_of_elements = filtered_elements.Count
f.log("Found {} strucutral elements in active selection.".format(count_of_elements)) 

if count_of_elements:
	filter_instance = filter_columns_and_beams()
	elements_with_wrong_reference = []
	
	f.log("\r\nVerifying Strucutral Connections host correctness...")
	counter = 1.0
	for el in filtered_elements:
		f.ProgresBarUpdate(counter/count_of_elements*30)
		f.Refresh()
		counter += 1
		if verify_reference(el,filter_instance) > 0.0052083:  # 1/16" tolerance
			elements_with_wrong_reference.append(el)
	
	
	incorrect_hosts_count = len(elements_with_wrong_reference)
	f.log("Command found {} elements with wrong reference".format(incorrect_hosts_count))

#newSelectionIds = List[ElementId]()
if incorrect_hosts_count:
	# make selection
	selectionList = List[ElementId]()
	selectionElementIds = [el.Id for el in elements_with_wrong_reference]
	map(selectionList.Add,selectionElementIds)
	uidoc.Selection.SetElementIds(selectionList)
	f.log("All of them have been selected")


	# task dialog to ask if proceed
	taskDialog = TaskDialog("Host Verification")
	taskDialog.CommonButtons = TaskDialogCommonButtons.Yes | TaskDialogCommonButtons.No
	taskDialog.DefaultButton = TaskDialogResult.Yes
	taskDialog.MainInstruction = "Found {} element(s) with incorrect host.".format(incorrect_hosts_count)
	taskDialog.MainContent = "Do you want to fix them? It will take about {} seconds.".format(incorrect_hosts_count + 10)
	answer = taskDialog.Show()

	correct_reference = []
	distance_to_new_face = []
	
	
	# PERFORM FIXING
	if answer == TaskDialogResult.Yes:
		selectionList.Clear()
		t = Transaction(doc)
		t.Start("Host Verification")	
		f.log("Fixing begins")
		#f.log("Creating 3D view for ray detection")
		f.ProgresBarUpdate(31)
	
		view_3d = create_3d_view()
		
		#f.log("Creating ray factory")
		projection = ReferenceIntersector(filter_instance, FindReferenceTarget.Face, view_3d)
		projection.FindReferencesInRevitLinks = False
		
		#f.log("\r\nRay detection and elements cloning starts:\r\n")
		
		#counter = 1.0
		newElementIds = List[ElementId]()
		
	
		for el in elements_with_wrong_reference:
			detected = detect_face_reference(el, projection)
			if detected[0] and detected[1]<0.0052083:   # distance tolerance 1/16" hardcoded
				correct_reference.append(detected[0])
				distance_to_new_face.append(detected[1])
			else:
				#detected2 = None
				detected2 = face_reference_by_boundling_box(el,filter_instance)
				if detected2[0] and detected2[1]<0.0052083:
					correct_reference.append(detected2[0])
					distance_to_new_face.append(detected2[1])
				else:
					correct_reference.append(None)
					distance_to_new_face.append(None)
					#f.log("Not found reference for {} element".format(el.Id))

			
		results = list(zip(elements_with_wrong_reference,correct_reference,distance_to_new_face))
		
		f.log("\r\n\r\nRESULTS:")
		counter = 1.0
		elementIdsToDelete = List[ElementId]()
		elementIdsToDelete.Add(view_3d.Id)
		count_of_elements = len(results)
		for el, refe, dist in results:
			if refe:
				el_from_ref = doc.GetElement(refe) # element from reference
				new_element = clone_element_on_face(el,refe)
				f.log("Element with wrong host: \t{}\tcorrect host: \t{}\t, distance: \t{}\t New element id:\t{}".format(el.Id,el_from_ref.Id,dist,new_element.Id))
				
				selectionList.Add(new_element.Id)
				
				elementIdsToDelete.Add(el.Id)
			
			else:
				f.log("Correct Reference for \t{}\t element NOT FOUND.".format(el.Id))
	
			f.ProgresBarUpdate(30+(counter/count_of_elements*70))
			counter += 1
			
		doc.Delete(elementIdsToDelete)	
		t.Commit()
		
		
		
	uidoc.Selection.SetElementIds(selectionList)
	TaskDialog.Show("Host Verification", "Finieshed. Elements selected.", TaskDialogCommonButtons.Ok)

f.ProgresBarUpdate(100)
f.Refresh()

f._button1.Enabled = False