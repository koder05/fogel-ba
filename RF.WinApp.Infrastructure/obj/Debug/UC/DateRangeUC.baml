   M S B A M L   `   `   `  ����S  ORF.WinApp.Infrastructure, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:RF.WinApp	RF.WinApp  5      +clr-namespace:RF.WinApp.UCRF.WinApp.UC  5          RF.WinApp.UC.DateRangeUC   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      &vcclr-namespace:RF.WinApp     5      )ucclr-namespace:RF.WinApp.UC     5      .��5
      ��   �� Orientation$  
Horizontal=�6   $��18��6*   $
��Left=�66   .+�5   
   �� $��с��6   $��18��6   $��10��6)      RF.WinApp.UC.DateInputUC  5   
   -   5          Name$ mdpDateFrom��5         Validation_ErrorTemplate =� ErrorTemplate# �   6.      	DateValue 5      ��  �� RelativeSource ��  �� Mode$ FindAncestor=� �� AncestorType$ uc:DateRangeUCL� �� Path$ FromDate� �� Mode$ TwoWay=�	 � BindingGroup	 5      &   System.Windows.Data.BindingGroup  5      
   Name$
 mdpDateFromBindingGroup��6      NotifyOnValidationError . 6>   5      �� 5   
   $
��по��6   $��18��6   $��30��6*   $��0,0,5,0q�65    �� TextAlignment$ Right=�6F     5   
   -   5        $ 	mdpDateTo��5      # �   6,    5      ��  �� $ FindAncestor=�$ uc:DateRangeUCL�$ TillDate�$ TwoWay=�	 5        5      $
 mdpDateToBindingGroup��6    . 6<   5      5      5      