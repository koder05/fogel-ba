   M S B A M L   `   `   `  ���� S  ORF.WinApp.Infrastructure, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullG9clr-namespace:RF.WinApp;assembly=RF.WinApp.Infrastructure	RF.WinApp  K GRF.WinApp.Assets, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:RF.WinApp	RF.WinApp M<clr-namespace:RF.WinApp.UC;assembly=RF.WinApp.InfrastructureRF.WinApp.UC  ?$clr-namespace:RF.WinApp.Assets.ViewsRF.WinApp.Assets.Views '   !RF.WinApp.Assets.Views.AssetsView   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     Hvc9clr-namespace:RF.WinApp;assembly=RF.WinApp.Infrastructure     'vc2clr-namespace:RF.WinApp     Kuc<clr-namespace:RF.WinApp.UC;assembly=RF.WinApp.Infrastructure     6views$clr-namespace:RF.WinApp.Assets.Views        RF.WinApp.ViewName    Title$  СЧА и ДДС��.��� -    �Name$	 ppp�� � RowDefinitions �� $��50 ��� $��* �.+��� $��1,1,1,0q����#eeeeee
�������$��Stretch=�$��Stretch=�._���  �� Orientation$ 
Horizontal=�.+��� -   $��90��   ToolBarButtonBaseStyle#���   .���  �� �� $��15 �.+��� $9��3/RF.WinApp.Assets;component/Img/File-Excel-icon.png��$��30��$��30��$��Center=��� $��1��$��Импорт из Excel�� �� TextAlignment$ Center=��� -   $��70��#���   .���  �� �� $��15 �.+��� $;��5/RF.WinApp.Assets;component/Img/export-excel-icon.png��$��23��$��23��$��Center=��� $��1��$��
Отчет��$ Center=�   RF.WinApp.CrudCC  -     Name$ 
AssetsCRUD��   DataViewProvider vc2:AssetsDataViewProvider$��1��   FilterBlockWidth$	 450Q�   FilterBlockHeight$	 200Q�	   FormEditBlockWidth$		 350Q�
   FormEditBlockHeight$	
 250Q� � 	Resources   frm
 �� Top �� Left   RF.WinApp.UC.MoneyUC   
MoneyValue �� Path �� Mode �� NotifyOnSourceUpdated �� UpdateSourceTrigger �� NotifyOnValidationError �� DisplayMemberPath �� SelectedValuePath �� SelectedValue �� RelativeSource �� Mode �� AncestorType   RF.WinApp.UC.DateInputUC  Validation_ErrorTemplate =� ErrorTemplate   	DateValue  RF.WinApp.GovernorLookup  Name   RF.WinApp.CrudLookup   LookupWidth   IsLookup  LookupWithExtension   LookupStyle    LookupValue! �� ValidationRules-   'RF.WinApp.DataAnnotationsValidationRule,  &System.Windows.Controls.ValidationRule"   ValidationStep��%*  &	      �� $	��100��$	��100��.+��� $-��'Управляющая компания��$��18��$	��125��$ 10��$��0q��� $��Дата��$��18��$	��120��$ 40��$��0q��� $#��Тип страхования��$��18��$	��120��$ 70��$��0q��� $��СЧА��$��18��$	��120��$	 100��$��0q��� $��ДДС��$��18��$	��120��$	 130��$��0q��� $	 130��.+�  $	 100�� �� $ Model.Value�$ TwoWay=� . $ PropertyChanged=� .   $	 130�� �� $ Model.CashFlow�$ TwoWay=� . $ PropertyChanged=� . �� $
 Name��$ Value��$ 70��$	��110�� �� $ Model.InsuranceType� . $ PropertyChanged=� . ����  �� $ FindAncestor=�$ UserControlL�$ InsuranceTypes�  $ 40��# �   �� $ Model.TakingDate�$ TwoWay=� . $ PropertyChanged=� .   $ AssetsView_GovernorLookup��$	 150Q�$	��350��$	��200��$ 10�� . # �    �� $ Model.Governor�$ PropertyChanged=�$ TwoWay=� .  . !   $" CommittedValue=�&	   System.Windows.Controls.DataGrid# 	  Columns# 0
  *System.Windows.Controls.DataGridTextColumn
  ,  &System.Windows.Controls.DataGridColumn$   Header$
$ УК��%   Width5  /System.Windows.Controls.DataGridLengthConverter$	% 150 1  +System.Windows.Controls.DataGridBoundColumn&   Binding& �� $ Model.Governor.ShortName�
  $#$ Тип страхования��$	% 150 & �� $ Model.InsuranceTypeString�
  $$ Дата��$	% 150 & �� $ Model.TakingDate�' �� StringFormat$' {0:d}��
  $$ СЧА��$	% 150 & �� $ Model.Value�$' n��
  $$ ДДС��$	% 150 & �� $ Model.CashFlow�$' {0:n2}��(   Filter( �� .���� -   ) ��Name$) C1��-  'System.Windows.Input.KeyboardNavigation*   TabNavigation$* Cycle=�.+��� $-��'Управляющая компания��$��18��$	��125��$ 10��$��0q��� $��Дата��$��18��$	��120��$ 40��$��0q��� $#��Тип страхования��$��18��$	��120��$ 70��$��0q��� -   $) C2��$	 125��.+��� -   + ��Name$+ cbInsuranceType��$
 Name��$ Value��$ 70��$	��110������  �� $ FindAncestor=�$ UserControlL�$ InsuranceTypes�   RF.WinApp.UC.DateRangeUC  -   ,  Name$, drDate��$ 40��$	��300��  -	   $ crudGovernors��$	 110Q�$	��350��$	��200��$ 10�� .    RF.WinApp.FilterCC  -   OperatorType$- Equals=�.   	FieldName$. 
GovernorId��/ � DataContext/ �� 0 �� ElementName$0 crudGovernors��$! LookupControl.SelectedModel�$ TwoWay=�1   Value1 �� *Id+  $- Equals=�$. InsuranceTypeValue��/ �� $0 cbInsuranceType��$ SelectedItem�$ TwoWay=�1 �� *Index+  $- MoreOrEquals=�$. 
TakingDate��1 �� $0 drDate��$ FromDate�  $- LessOrEquals=�$. 
TakingDate��1 �� $0 drDate��$ TillDate�2   FormEdit2 �� #��[ 3   FormNew3 �� #��[    RF.WinApp.ActionBlock  -
   4  Name$4 	xlsImport��$	��320��$	��300��$��2��5   BlockCaption$*5 $Импорт данных из Excel��6   ApplyCaption$6 Импорт��7   ClearCaption$7 Отмена��8   ModalScopeElement8 �� $	0 ppp�� 9 �� 
TargetType: �� Property; �� Value< �� 
TargetType= ��Name  ToggleButton> ��Name? F�Name@ �� TriggersA P� PropertyB P� ValueC �� 
TargetName  ValidationErrorD � ToolTip��%0  '��       &	 �     &	     �� "9 ��.��� : �@J�SnapsToDevicePixels; . �� : �@�OverridesDefaultStyle; .  �� : � ��$; 0,0,0,0q��� : � ��$; 18���� : � ��$	; 250���� : � ��; �� "< ��.R��� $= PART_ContentHost��$��0q��� "< ��.R��� $> Border��$��1,0,0,0q�
�������
�������$��24��._�F� $? Arrow��
�������$��Center=�$��Center=�"�����{��    р�  	= р�[     	@ P� A �@J�IsMouseOverB . . ��� $C Border��: � ��
; ������� $C Arrow��: � ��
; ������� "< ��.R�]� ��.  .+��� $��Right=�$��2,0,0,0q�$7��1/RF.WinApp.Infrastructure;component/Img/error.png��$��14��$��14��D �� $0 	MyAdorner��$8 2AdornedElement.(Validation.Errors)[0].ErrorContent��� 
���  ��$��1q�$	��276��._��� $��	MyAdorner������ E �� Color$E 	#FF688CAF��F �� Opacity$F .3Q�.���� .+��� $��1q�
�������
�������G J� SnapsToDevicePixelsG . ._��� $ 
Horizontal=�.+�	� -   $��rbImportValue��H 	� 	GroupName$H rbImportType��$��СЧА��$��10q�I d� 	IsChecked$
I TrueY�	� -   $��rbImportCashFlow��$H rbImportType��$��ДДС��$��10q��� $��Файл Excel��$��18��$	��120��$��0q�$
��Left=�$��0,10,0,0q�   RF.WinApp.ActionTextBox  -   J  Name$J tbImportFileName��$
��Left=�$	��250��K �� 
IsReadOnlyK . # �  '  !RF.WinApp.Assets.Views.AssetsViewL   FileBrowseActionM   Action#M ZL    RF.WinApp.AdornedPopupN   IsShowN �� $0 aPopup��$ (vc:AdornedPopup.IsShow)�$ TwoWay=�.������ $ 	ExcelFile�$ PropertyChanged=�$ TwoWay=� .  . !   $" CommittedValue=�.���� $&�� Лист Excel с данными��$��18��$	��120��$��0q�$��0,10,0,0q�$
��Left=��� -   $+ cbDataSheet��$	��250��$
��Left=�O �� SelectedItemO �� $ ExcelDataSheet�$ TwoWay=�!   $" CommittedValue=��� $#��Тип страхования��$��18��$	��120��$��0q�$��0,10,0,0q�$
��Left=��� -   $+ cbImportInsuranceType��$
 Name��$ Value��$ 70��$	��110��$
��Left=�����  �� $ FindAncestor=�$ UserControlL�$ InsuranceTypes�O �� $ InsuranceType�$ TwoWay=�!   $" CommittedValue=�  -   $4 
reportForm��$	��350��$	��300��$��2��$%5 Параметры отчета��$6 
Отчет��$7 Отмена��8 �� $	0 ppp������ $E 	#FF688CAF��$F .3Q�.���� $* 	Contained=�.+��� $#��Тип страхования��$��18��$	��120��$��0q�$��0,10,0,0q�$
��Left=��� -   $+ cbReportInsuranceType��$
 Name��$ Value��$	��110��$
��Left=�P   TabIndex$P 0������  �� $ FindAncestor=�$ UserControlL�$ InsuranceTypes�O �� $ InsuranceType�$ TwoWay=�!   $" CommittedValue=��� $'��!Период построения��$��18��$	��120��$��0q�$
��Left=�$��0,10,0,0q�  -   $, drReportPeriod��$	��300��Q   FromDateQ �� $ 	DateBegin�$ TwoWay=� . !   $" CommittedValue=�R   TillDateR �� $ DateEnd�$ TwoWay=� . !   $" CommittedValue=��� $-��'Управляющая компания��$��18��$��0q�$
��Left=�$��0,10,0,0q�  -   $ crudGovernorsRep��$	 110Q�$	��350��$	��200�� .   �� *
Governor+$ TwoWay=�