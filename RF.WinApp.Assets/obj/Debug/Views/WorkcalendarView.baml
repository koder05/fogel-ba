   M S B A M L   `   `   `  ����S  ORF.WinApp.Infrastructure, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullG9clr-namespace:RF.WinApp;assembly=RF.WinApp.Infrastructure	RF.WinApp  5      K GRF.WinApp.Assets, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:RF.WinApp	RF.WinApp 5      ?$clr-namespace:RF.WinApp.Assets.ViewsRF.WinApp.Assets.Views 5	      M<clr-namespace:RF.WinApp.UC;assembly=RF.WinApp.InfrastructureRF.WinApp.UC  5
      O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem 5      -   'RF.WinApp.Assets.Views.WorkcalendarView   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Hvc9clr-namespace:RF.WinApp;assembly=RF.WinApp.Infrastructure     5      'vc2clr-namespace:RF.WinApp     5      4vc3$clr-namespace:RF.WinApp.Assets.Views     5	      Kuc<clr-namespace:RF.WinApp.UC;assembly=RF.WinApp.Infrastructure     5
      6sys&clr-namespace:System;assembly=mscorlib     5         RF.WinApp.ViewName    Title$'  !Рабочий календарь��5      .��5         RF.WinApp.CrudCC  -   5          Name$ WorkcalendarCRUD��5         DataViewProvider$  vc2:WorkcalendarDataViewProvider6*      FilterBlockWidth$	 412Q�5         FilterBlockHeight$	 150Q�6+      FormEditBlockWidth$	 400Q�6C      FormEditBlockHeight$	 250Q�6\    � 	Resources 5   
      frm&   System.Windows.Controls.Calendar   SelectionMode	   DisplayDate
 �� Path �� Mode   SelectedDate J� Opacity �� Left
 �� Top d� 	IsChecked �� NotifyOnSourceUpdated �� UpdateSourceTrigger �� TextWrapping  Validation_ErrorTemplate =� ErrorTemplate �� NotifyOnValidationError �� ValidationRules-   'RF.WinApp.DataAnnotationsValidationRule,  &System.Windows.Controls.ValidationRule   ValidationStep��%�  &	        �� 5      .+�5        $ 
SingleDate=�5      $
��Left=�5      $��1,1,1,1q�65   
�������	 5      �� $
 
Model.Date�$ OneWay=� 6P   �� $
 
Model.Date�$ OneWay=�5   O   �� 5      ���#eeeeee$	 0.2Q�6.   $	��175��6<   $	��160��6H   �� 5      $	 190��6   $ 10��6+   .+�5      �� $%��Является рабочим��6   $��18��67   $	��120��6C   $ 10��6O   $��0q�6_   �� 5      $ 10��6�   $	 110��6�    6   �� $
 Model.IsWorkingDay� . $ PropertyChanged=�$ TwoWay=�6�   �� 5      $��Комментарий��6   $��18��62   $	��120��6>   $ 50��6J   $��0q�6Z   �� 5      $ 70��6   $	��180��6.   $��50��6:   $
 Wrap=�6F   # �  6Z   .��6�   ��5      �� 5      $
 Model.Comment�6&   $ PropertyChanged=�6;   $ TwoWay=�6a    .  .  5   "     5   &   $ CommittedValue=�6G   5       .��5!   (   5"      5#      5$      5%      &   System.Windows.Controls.DataGrid   Columns 5&   
   0  *System.Windows.Controls.DataGridTextColumn  5'      ,  &System.Windows.Controls.DataGridColumn   Header$ Дата��6!      Width5	  /System.Windows.Controls.DataGridLengthConverter$	 150	 6r   1
  +System.Windows.Controls.DataGridBoundColumn 
  Binding 6/   �� $
 
Model.Date� �� StringFormat$ {0:dd.MM.yyyy}��6r     5(      $ Раб/вых��6!   $	 150	 6Y    62   �� $
 Model.DayType�6Y     5)      $ Комментарий��6!   $	 350	 6]    66   �� $
 Model.Comment�6]      Filter 5+   
   �� 5,      .��5-      �� -  'System.Windows.Input.KeyboardNavigation   TabNavigation$ Cycle=�6   .+�5.      �� $%��Полный календарь��6   $��18��67   $	��120��6C   $ 10��6O   $��0q�6_   $��0��6k   �� 5/      $��Дата��6   $��18��6+   $	��120��67   $ 40��6C   $��0q�6S   $��2��6_   �� 50      $	 125��6   .+�51      �� -   5         ��Name$ chbDefCondition��51   E   $ 10��6#   $ FalseY�63      RF.WinApp.ActionBlock    ApplyAction#��Z  6^   $��1��6�      RF.WinApp.UC.DateRangeUC  52      -   5        !  Name$! drDate��52   E   $ 40��6)   $	��300��69   $��3��6U   -  'RF.WinApp.Assets.Views.WorkcalendarView"   DefaultFromDate#   FromDate## Z" 53   )   $ �� Today%   TillDate#% Z$ 6d   54         RF.WinApp.FilterCC  55      &   OperatorType$& 	Condition=�6"   '   	FieldName$' IsWorkingDay��6;   (   Value( 6T   �� ) �� ElementName$) chbDefCondition��$
 	IsChecked�  56      $& MoreOrEquals=�6"   $
' Date��6>   ( 6O   �� $) drDate��$
 FromDate�  57      $& LessOrEquals=�6"   $
' Date��6>   ( 6O   �� $) drDate��$
 TillDate�58      59      *   FormEdit* 5;   
   �� 5<      #��[  6   5>      5?      