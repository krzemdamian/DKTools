#region Copyright (c) 2009-2011 Arup, All rights reserved.
//
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
//
#endregion
#region File Information
//
// Filename: VisibilityConverter.cs
// Author: Yamin Tengono <Yamin.Tengono@arup.com.au>
//
// This file is part of Arup Common Library.
//
#endregion

using System;
using System.Windows.Data;
using System.Globalization;

namespace rvtUnit.Helpers
{

   /// <summary>
   /// Visiblity value converter for xaml.
   /// </summary>
   [ValueConversion(typeof(bool), typeof(string))]
   public class VisibilityConverter : IValueConverter
   {
      // ======================================================= METHODS === //

      /// <summary>
      /// Converts a value.
      /// </summary>
      /// <param name="value">The value produced by the binding source.</param>
      /// <param name="targetType">The type of the binding target property.</param>
      /// <param name="parameter">The converter parameter to use.</param>
      /// <param name="culture">The culture to use in the converter.</param>
      /// <returns>A converted value.</returns>
      public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
      {
         bool isVisible = (bool)value;

         if (isVisible)
         { return "Visible"; }
         else
         { return "Collapsed"; }
      }

      /// <summary>
      /// Converts back to value.
      /// </summary>
      /// <param name="value"></param>
      /// <param name="targetType"></param>
      /// <param name="parameter"></param>
      /// <param name="culture"></param>
      /// <returns></returns>
      public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
      {
         return null;
      }

   }  // End of class VisibilityConverter

}  // End of namespace Arup.AADG.ACL.Controls.WPF
