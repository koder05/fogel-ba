   M S B A M L   `   `   `  ���� S  ORF.WinApp.Infrastructure, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullG9clr-namespace:RF.WinApp;assembly=RF.WinApp.Infrastructure	RF.WinApp  K GRF.WinApp.Assets, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:RF.WinApp	RF.WinApp ?$clr-namespace:RF.WinApp.Assets.ViewsRF.WinApp.Assets.Views M<clr-namespace:RF.WinApp.UC;assembly=RF.WinApp.InfrastructureRF.WinApp.UC  O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem -   'RF.WinApp.Assets.Views.WorkcalendarView   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     Hvc9clr-namespace:RF.WinApp;assembly=RF.WinApp.Infrastructure     'vc2clr-namespace:RF.WinApp     4vc3$clr-namespace:RF.WinApp.Assets.Views     Kuc<clr-namespace:RF.WinApp.UC;assembly=RF.WinApp.Infrastructure     6sys&clr-namespace:System;assembly=mscorlib        RF.WinApp.ViewName    Title$'  !Рабочий календарь��.��   RF.WinApp.CrudCC  -     Name$ WorkcalendarCRUD��   DataViewProvider$  vc2:WorkcalendarDataViewProvider   FilterBlockWidth$	 412Q�   FilterBlockHeight$	 150Q�   FormEditBlockWidth$	 400Q�   FormEditBlockHeight$	 250Q� � 	Resources    frm&   System.Windows.Controls.Calendar   SelectionMode	   DisplayDate
 �� Path �� Mode   SelectedDate J� Opacity �� Left
 �� Top d� 	IsChecked �� NotifyOnSourceUpdated �� UpdateSourceTrigger �� TextWrapping  Validation_ErrorTemplate =� ErrorTemplate �� NotifyOnValidationError �� ValidationRules-   'RF.WinApp.DataAnnotationsValidationRule,  &System.Windows.Controls.ValidationRule   ValidationStep��%�  &	        �� .+�  $ 
SingleDate=�$
��Left=�$��1,1,1,1q�
�������	 �� $
 
Model.Date�$ OneWay=� �� $
 
Model.Date�$ OneWay=��� ���#eeeeee$	 0.2Q�$	��175��$	��160���� $	 190��$ 10��.+��� $%��Является рабочим��$��18��$	��120��$ 10��$��0q��� $ 10��$	 110�� �� $
 Model.IsWorkingDay� . $ PropertyChanged=�$ TwoWay=��� $��Комментарий��$��18��$	��120��$ 50��$��0q��� $ 70��$	��180��$��50��$
 Wrap=�# �  .������ $
 Model.Comment�$ PropertyChanged=�$ TwoWay=� .  .    $ CommittedValue=�.��&   System.Windows.Controls.DataGrid   Columns 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn   Header$ Дата��   Width5	  /System.Windows.Controls.DataGridLengthConverter$	 150	 1
  +System.Windows.Controls.DataGridBoundColumn 
  Binding �� $
 
Model.Date� �� StringFormat$ {0:dd.MM.yyyy}��  $ Раб/вых��$	 150	  �� $
 Model.DayType�  $ Комментарий��$	 350	  �� $
 Model.Comment�   Filter �� .���� -  'System.Windows.Input.KeyboardNavigation   TabNavigation$ Cycle=�.+��� $%��Полный календарь��$��18��$	��120��$ 10��$��0q�$��0���� $��Дата��$��18��$	��120��$ 40��$��0q�$��2���� $	 125��.+��� -    ��Name$ chbDefCondition��$ 10��$ FalseY�   RF.WinApp.ActionBlock    ApplyAction#��Z  $��1��   RF.WinApp.UC.DateRangeUC  -   !  Name$! drDate��$ 40��$	��300��$��3��-  'RF.WinApp.Assets.Views.WorkcalendarView"   DefaultFromDate#   FromDate## Z" $ �� Today%   TillDate#% Z$    RF.WinApp.FilterCC  &   OperatorType$& 	Condition=�'   	FieldName$' IsWorkingDay��(   Value( �� ) �� ElementName$) chbDefCondition��$
 	IsChecked�  $& MoreOrEquals=�$
' Date��( �� $) drDate��$
 FromDate�  $& LessOrEquals=�$
' Date��( �� $) drDate��$
 TillDate�*   FormEdit* �� #��[  