   M S B A M L   `   `   `  ���� S  ORF.WinApp.Infrastructure, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:RF.WinApp	RF.WinApp      RF.WinApp.CrudCCLookupW SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089)   #RF.WinApp.LookupFieldValueConverter   LookupFieldValueConverter(  "RF.WinApp.DataObjEnvelopeConverter  DataObjEnvelopeConverter  ColumnHeaderInLookup  �� 
TargetType=  7System.Windows.Controls.Primitives.DataGridColumnHeader �� Property �� Value �� 
TargetType � 	Resources ( $DataGridColumnHeader_MainBorderStyle �� ContentStringFormat �� ContentStringFormat & "DataGridColumnHeader_SortPathStyle E� 
StartPoint �� Point 4 0DataGridColumnHeader_PART_LeftHeaderGripperColor 5 1DataGridColumnHeader_PART_RightHeaderGripperColor	 �� Triggers
 P� Property P� Value �� 
TargetName  	RowHeader:  4System.Windows.Controls.Primitives.DataGridRowHeader  CellInLookup*  $System.Windows.Controls.DataGridCell J� SnapsToDevicePixels �� Triggers 	 SelectedItemListBrush 
 MouseOverListBrush �� Binding �� Path �� Value  ItemInLookup)  #System.Windows.Controls.DataGridRow   RF.WinApp.CrudCC  LookupWithExtension&   System.Windows.Controls.DataGrid	   RF.WinApp.ActionTextBox 	 Name   ToggleFormAction 	  Action  TextBoxWithAdornedPopup2   
IsReadOnly �� 
IsReadOnly  Validation_ErrorTemplate =� ErrorTemplate �� Mode �� RelativeSource
   RF.WinApp.AdornedPopup 
  IsShow �� ElementName � DataContext �� Mode 
 Name ��Name  DropDownPopupBorder  ��Name! � ColumnDefinitions" � RowDefinitions#   SelectAllCommand$ �� 
ResourceId% �� TypeInTargetAssembly7  1System.Windows.Controls.DataGridHeadersVisibility
&   All' �� ConverterParameter!(   HeadersVisibilityConverter) �� 	Converter* �� AncestorTypeG  ASystem.Windows.Controls.Primitives.DataGridColumnHeadersPresenter+  Name,   Column- ��Name. �� CanContentScroll/ ��Name0 �� ScrollableHeight1 � Maximum2 �� Orientation*3 �� #ComputedVerticalScrollBarVisibility4 �� ViewportHeight5 �� ViewportSize6 � Value7 �� ScrollableWidth,8 �� %ComputedHorizontalScrollBarVisibility9 �� ViewportWidth: ��Name   RF.WinApp.FormCC;  Name
< �� Top=   FormEditBlockWidth>   FormEditBlockHeight   RF.WinApp.ActionBlock?   BlockCaption@   ApplyCaptionA   ClearCaptionB   ModalScopeElementC �� AncestorLevelD � 	ResourcesE   FormEditF   FormNewBlockWidthG   FormNewBlockHeight   RF.WinApp.FilterBlockCCH  NameI P� 
SourceName 
 LookupJ ��Name  TextBoxWithAdornedPopup   D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    :x,http://schemas.microsoft.com/winfx/2006/xaml     vcclr-namespace:RF.WinApp  %)  &	        &	      &	 
     &	 �    &	 �    &	 �    &	 �    7  7  7  7  7  7  &	 "    7  7  7  7      �� "   .���  � ��$ Center=���  � ��$ 18����  � ��$ 10(���  � �� �� "   ���� 'n�       "  n�.���  � ��$ 1����  �@�Cursor$ SizeWE����  � �� �� " n�.R��� #��z��.R�� .+��� $��
MainBorder��#���  ._�� .+��� $��3q�$��Center=�$��Center=�#��z��# z #��z��F� $��UpArrow��#���  ��B� .��E� $	 0,0&�.���� $	 7,0&��� $ 3.5,6&��� $	 0,0&�F� $��	DownArrow��#���  ��B� .��E� $	 0,6&�.���� $	 7,6&��� $ 3.5,0&��� $	 0,6&�n� $��PART_LeftHeaderGripper��$
��Left=�#���  n� $��PART_RightHeaderGripper��$��Right=�#���  	 P� 
 �@ SortDirection$ 
DescendingX�. ��� $ UpArrow�� � y�$ Hidden=��� $ 	DownArrow�� � y�$ Visible=�P� 
 �@ SortDirection$ 	AscendingX�. ��� $ 	DownArrow�� � y�$ Hidden=��� $ UpArrow�� � y�$ Visible=��� "   .���  � ��$ 0����  � �� Z� �� "   .���  � ��$ Arial)���  � ��$ 10(���  � ��$ CharacterEllipsis=���  � ��
 �   ���  � ��$ 0q���  � ��$ 18����  � �� �� "  .R��� #��z��#��z��#��z�� . ._��� $��Center=�#��z��$��1,0,0,0q�#��z��# z # z  P� 
 �@ 
IsSelected . . ���  � ��# � 	 ��  � ��
 �   ���  � ��
 ���� P� 
 �@J�	IsFocused . . ���  � ��# � 
 ��  �� $ ID� 0.a���  � ��
 �������  � ��
 ������� "   .���  � ��$ 0,0,0,0q� P� 
 �@ AlternationIndex$ 0��. ���  � ��
 �����P� 
 �@ AlternationIndex$ 1��. ���  � �� �#EFF3EFP� 
 �@J�IsMouseOver . . ���  � �� �#FAE9C4�� "   .���  �@ AutoGenerateColumns .  ��  � ��$
 Left=���  � ��$	 Top=���  �@ HeadersVisibility$ Column=���  �@ RowHeaderStyle8   ��  �@ ColumnHeaderStyle8  ��  �@ 	CellStyle8  ��  � ��8  ��  �@��AlternationCount$ 2����  �@ GridLinesVisibility$ Vertical=���  �@ SelectionMode$ Extended=���  �@ ClipboardCopyMode$ ExcludeHeader=���  �@ VerticalGridLinesBrush �#C6D7C5��  �@ SelectionUnit$ FullRow=���  �@$�IsVirtualizing . �� " �@ EnableColumnVirtualization .  ��  �@ EnableRowVirtualization . ��  � �� . ��  �@=�ErrorTemplate Z� ��  � �� �� "  Crud��.R�� .+��� .+�	  $ PART_LookupBox��# Z $
��Left=�#���  # z # �  ���� $! (vc:CrudLookup.LookupWidth)�$ OneWay=� �� *TemplatedParent+ �� $ aPopup��$ (vc:AdornedPopup.IsShow)�$ TwoWay=� ��  �� $ TemplatedParent=�$ SelectedModel�$ TwoWay=�.��.��
  $ aPopup�� ��  �� *TemplatedParent+$ (vc:AdornedPopup.IsShow)�$ TwoWay=�.���� $ calDatePopupBorder��#���  $��0,1,0,0q�$
��Auto��#��z��$
��Auto��#��z��._��� $  DG_ScrollViewer��|�.  ���� " ��.R�� ! �� $
��Auto ��� $��* ��� $
��Auto �" �� $
��Auto ��� $��* ��� $
��Auto �.+��� #��Z# |�.  ��C� *�� $"$ DataGridSelectAllButtonStyle��"%  +y��� *HeadersVisibility+#' Z& #) Z(  �� "*  ���� *CellsPanelHorizontalOffset+ �� "*    $!+ PART_ColumnHeadersPresenter��$��1��y��� *HeadersVisibility+#' Z, #) Z(  �� "*  �� $!- PART_ScrollContentPresenter��#. z��$��2��$��1���� $/ PART_VerticalScrollBar��$��2��#1 z0 $2 Vertical=�$��1��#y�z3 #5 z4 6 �� *VerticalOffset+$ OneWay=� �� *TemplatedParent+� $��1��$��2��! �� ���� **(NonFrozenColumnsViewportHorizontalOffset+ �� "*  �� $��* �.+��� $/ PART_HorizontalScrollBar��$��1��#1 z7 $2 
Horizontal=�#y�z8 #5 z9 6 �� *HorizontalOffset+$ OneWay=� �� *TemplatedParent+.���� # z �� $: 
Lookup_ppp��.+�  $; PART_FormEditCC��$< 26��#��z= #��z> $"? Редактирование��$@ Сохранить��$A 
Сброс�� ��  �� $ TemplatedParent=�$ SelectedModel�8)  B ��  �� $ FindAncestor=�$C 2��$* 	vc:CrudCCL�D ���� '��       "  ��.���  � �� C� *�� "% ��$$ InThemeButtonBaseTemplate��+ ��  �� $ 	IsEditing� True.a���  � ��
 �  ��.���� #��zE   $; PART_FormNewCC��$< 26��#��zF #��zG $? Новая запись��$@ Сохранить��$A 
Сброс�� ��  �� $ TemplatedParent=�$ SelectedModel�8)  $ OneWayToSource=�B ��  �� $ FindAncestor=�$C 2��$* 	vc:CrudCCL�D ���� '��       "  ��.���  � �� C� *�� "% ��$$ InThemeButtonBaseTemplate��+ ��  �� $ 	IsEditing� True.a���  � ��
 �  ��.���� $��FormNew��  $H PART_FilterBlockCC��.���� $��Filter��	 P� $I aPopup��
 �@
 IsShow .  . ���  �@ 
IsBindable .  P� 
 �@=�HasError . . ��� $ PART_LookupBox�� ��  �� *TemplatedParent+$ (Validation.Errors)[0]� �@
 Err�� "   .���  �@ AutoGenerateColumns .  ��  � ��$
 Left=���  � ��$	 Top=���  �@ HeadersVisibility$ Column=���  �@ RowHeaderStyle8   ��  �@ ColumnHeaderStyle8  ��  �@ 	CellStyle8  ��  � ��8  ��  �@��AlternationCount$ 2����  �@ GridLinesVisibility$ Vertical=���  �@ SelectionMode$ Extended=���  �@ ClipboardCopyMode$ ExcludeHeader=���  �@ VerticalGridLinesBrush �#C6D7C5��  �@ SelectionUnit$ FullRow=���  �@$�IsVirtualizing . �� " �@ EnableColumnVirtualization .  ��  �@ EnableRowVirtualization . ��  � �� . ��  �@=�ErrorTemplate Z� ��  � �� �� "  Crud��.R�� .+��� .+��� $J PART_LookupBox��$
��Left=�#���  # z # �  ���� $! (vc:CrudLookup.LookupWidth)�$ OneWay=� �� *TemplatedParent+ �� $ aPopup��$ (vc:AdornedPopup.IsShow)�$ TwoWay=� ��  �� $ TemplatedParent=�$ SelectedModel�$ TwoWay=�
  $ aPopup�� ��  �� *TemplatedParent+$ (vc:AdornedPopup.IsShow)�$ TwoWay=�.���� $ calDatePopupBorder��#���  $��0,1,0,0q�$
��Auto��#��z��$
��Auto��#��z��._��� $  DG_ScrollViewer��|�.  ���� " ��.R�� ! �� $
��Auto ��� $��* ��� $
��Auto �" �� $
��Auto ��� $��* ��� $
��Auto �.+��� #��Z# |�.  ��C� *�� $"$ DataGridSelectAllButtonStyle��"%  +y��� *HeadersVisibility+#' Z& #) Z(  �� "*  ���� *CellsPanelHorizontalOffset+ �� "*    $!+ PART_ColumnHeadersPresenter��$��1��y��� *HeadersVisibility+#' Z, #) Z(  �� "*  �� $!- PART_ScrollContentPresenter��#. z��$��2��$��1���� $/ PART_VerticalScrollBar��$��2��#1 z0 $2 Vertical=�$��1��#y�z3 #5 z4 6 �� *VerticalOffset+$ OneWay=� �� *TemplatedParent+� $��1��$��2��! �� ���� **(NonFrozenColumnsViewportHorizontalOffset+ �� "*  �� $��* �.+��� $/ PART_HorizontalScrollBar��$��1��#1 z7 $2 
Horizontal=�#y�z8 #5 z9 6 �� *HorizontalOffset+$ OneWay=� �� *TemplatedParent+.���� # z   $H PART_FilterBlockCC��.���� 	 P� $I aPopup��
 �@
 IsShow .  . ���  �@ 
IsBindable .  P� 
 �@=�HasError . . ��� $ PART_LookupBox�� ��  �� *TemplatedParent+$ (Validation.Errors)[0]� �@
 Err