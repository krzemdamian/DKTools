using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using RevitDKTools.DockablePanels.ParameterEditor.Model;
//using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Drawing;
using System.Text.RegularExpressions;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;
using Autodesk.Revit.ApplicationServices;

public class SelectionChangedWatcher
{
    public event EventHandler SelectionChanged;

    /// <summary>
    /// Auto-implemented property storing a list 
    /// of all currently selected elements.
    /// </summary>
    public List<ElementId> Selection
    {
        get;
        set;
    }

    private List<int> _lastSelIds;

    public SelectionChangedWatcher(UIControlledApplication a)
    {
        a.Idling += new EventHandler<IdlingEventArgs>(OnIdling);
    }

    private void OnIdling(object sender,IdlingEventArgs e)
    {
        // Idling events happen when the application has 
        // nothing else to do,
        // They can happen very frequently and the user 
        // will experience a lag if this code takes a 
        // significant amount of time to execute.

        UIApplication uiApplication = (UIApplication)sender;

        ICollection<ElementId> selected = uiApplication
          .ActiveUIDocument.Selection.GetElementIds();

        
        if (0 == selected.Count)
        {
            if (null != Selection && 0 < Selection.Count)
            {
                // if something was selected previously, and 
                // now the selection is empty, report change

                HandleSelectionChange(selected);
            }
        }
        else // elements are selected
        {
            if (null == Selection)
            {
                // previous selection was null, report change

                HandleSelectionChange(selected);
            }
            else
            {
                if (Selection.Count != selected.Count)
                {
                    // size has changed, no need to check 
                    // selection IDs, report the change

                    HandleSelectionChange(selected);
                }
                else
                {
                    // count is the same... 
                    // compare IDs to see if selection has changed
                    if (SelectionHasChanged(selected))
                    {
                        HandleSelectionChange(selected);
                    }
                }
            }
        }
    }

    private bool SelectionHasChanged(
      ICollection<ElementId> selected)
    {
        // we have already determined that the size of 
        // "selected" is the same as the last selection...

        int i = 0;
        foreach (ElementId e in selected)
        {
            if (_lastSelIds[i] != e.IntegerValue)
            {
                return true;
            }
            ++i;
        }
        return false;
    }

    private void HandleSelectionChange(
      ICollection<ElementId> selected)
    {
        // store the current list of elements in the 
        // Selection property and populate _lastSelIds 
        // with the current selection's ids

        Selection = new List<ElementId>();
        _lastSelIds = new List<int>();

        foreach (ElementId e in selected)
        {
            Selection.Add(e);
            _lastSelIds.Add(e.IntegerValue);
        }
        Call_SelectionChanged();
    }

    private void Call_SelectionChanged()
    {
        if (SelectionChanged != null)
        {
            SelectionChanged(this, new EventArgs());
        }
    }
}