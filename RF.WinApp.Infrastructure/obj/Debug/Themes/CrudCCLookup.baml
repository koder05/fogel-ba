   M S B A M L   `   `   `  ����S  ORF.WinApp.Infrastructure, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:RF.WinApp	RF.WinApp  5          RF.WinApp.CrudCCLookupW SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089)   #RF.WinApp.LookupFieldValueConverter   LookupFieldValueConverter(  "RF.WinApp.DataObjEnvelopeConverter  DataObjEnvelopeConverter  ColumnHeaderInLookup  �� 
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
 LookupJ ��Name  TextBoxWithAdornedPopup   5      D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    :x,http://schemas.microsoft.com/winfx/2006/xaml     5      vcclr-namespace:RF.WinApp  5      %.  &	        &	      &	 &     &	 �    &	 5    &	 �
    &	 m    7  7  7  7  7  7  &	 �    7  7  7  7    5      6#     5      6"   �� 5
      "   6)   .�5   
   ��  � ��$ Center=�65   �� 5   
    � ��$ 18��6#   �� 5   
    � ��$ 10(�6%   �� 5   
    � �� 5      �� 5      "  6"    5      ���� 5      'n�       "  n�6    .�5      ��  � ��$ 1��6G   �� 5       �@�Cursor$ SizeWE��6H   �� 5       � �� 5   "   �� 5   &   " n�66   .R�5   *   �� #��z��61   5   '   5      5      5      .R�5      � .+�5      �� $��
MainBorder��6!   #���  63   ._�5       � .+�5!   "   �� $��3q�63   $��Center=�6>   $��Center=�6[   #��z��6v   # z 5"   7   #��z��6s   5#      5$      F� 5%      $��UpArrow��6   #���  6.   ��5&      B� 5'   "   .��5(   &   E� $	 0,0&�61   .��5)   *   �� $	 7,0&�66   �� 5*   *   $ 3.5,6&�66   �� 5+   *   $	 0,0&�66   5,   '   5-   #   5/      F� 50      $��	DownArrow��6   #���  60   ��51      B� 52   "   .��53   &   E� $	 0,6&�61   .��54   *   �� $	 7,6&�66   �� 55   *   $ 3.5,0&�66   �� 56   *   $	 0,6&�66   57   '   58   #   5:      n� 5;      $��PART_LeftHeaderGripper��6    $
��Left=�6>   #���  6Y   n� 5<      $��PART_RightHeaderGripper��6    $��Right=�6?   #���  6[   5=      	 5>      P� 5?      
 �@ SortDirection$ 
DescendingX�6;   . �5@      �� $ UpArrow��6%    � y�$ Hidden=�6P   �� 5A      $ 	DownArrow��6%    � y�$ Visible=�6R   5B      P� 5C      
 �@ SortDirection$ 	AscendingX�6;   . �5D      �� $ 	DownArrow��6%    � y�$ Hidden=�6R   �� 5E      $ UpArrow��6%    � y�$ Visible=�6P   5F      5H      5J      5K      �� 5M      "   6   .�5N   
   ��  � ��$ 0��6"   �� 5O   
    � �� 6%   Z� 5P      �� 5R      "   6!   .�5S   
   ��  � ��$ Arial)�61   �� 5T   
    � ��$ 10(�6/   �� 5U   
    � ��$ CharacterEllipsis=�63   �� 5V   
    � ��
 �   �61   �� 5W   
    � ��$ 0q�6,   �� 5X   
    � ��$ 18��6#   �� 5Y   
    � �� 5Z      �� 5[      "  6"   .R�5\      �� #��z��6   #��z��5]      #��z��5^       . ._�5_      �� $��Center=�6+   #��z��5`   #   $��1,0,0,0q�5a   #   #��z��5b   #   # z 5c   #   # z 5d   #   5e      5f      5h       5i   
   P� 5j      
 �@ 
IsSelected . . �5k      ��  � ��# � 	 6/   �� 5l       � ��
 �   �6/   �� 5m       � ��
 ���� 60   5n      P� 5o      
 �@J�	IsFocused . . �5p      ��  � ��# � 
 6/   5q      �� 5r       6$   �� $ ID� 06   .a�5s      ��  � ��
 �����6/   �� 5t       � ��
 �����6/   5u      5w      �� 5y      "   6!   .�5z   
   ��  � ��$ 0,0,0,0q�6#    5{   
   P� 5|      
 �@ AlternationIndex$ 0��62   . �5}      ��  � ��
 �����6/   5~      P� 5      
 �@ AlternationIndex$ 1��62   . �5�      ��  � �� �#EFF3EF6/   5�      P� 5�      
 �@J�IsMouseOver . . �5�      ��  � �� �#FAE9C46/   5�      5�      �� 5�      "   6   .�5�   
   ��  �@ AutoGenerateColumns .  60   �� 5�   
    � ��$
 Left=�60   �� 5�   
    � ��$	 Top=�6.   �� 5�   
    �@ HeadersVisibility$ Column=�6.   �� 5�   
    �@ RowHeaderStyle8   6+   �� 5�   
    �@ ColumnHeaderStyle8  6.   �� 5�   
    �@ 	CellStyle8  6&   �� 5�   
    � ��8  6/   �� 5�   
    �@��AlternationCount$ 2��6-   �� 5�   
    �@ GridLinesVisibility$ Vertical=�60   �� 5�   
    �@ SelectionMode$ Extended=�6*   �� 5�   
    �@ ClipboardCopyMode$ ExcludeHeader=�6.   �� 5�   
    �@ VerticalGridLinesBrush �#C6D7C563   �� 5�   
    �@ SelectionUnit$ FullRow=�6*   �� 5�   
    �@$�IsVirtualizing . 6B   �� 5�   
   " �@ EnableColumnVirtualization .  67   �� 5�   
    �@ EnableRowVirtualization . 64   �� 5�   
    � �� . 6:   �� 5�   
    �@=�ErrorTemplate 65   Z� �� 5�   
    � �� 5�      �� 5�      "  6"   Crud��.R�5�      � .+�5�      �� .+�5�      	  $ PART_LookupBox��6/   # Z 5�   %   $
��Left=�5�   %   #���  5�   %   # z 5�   %   # �  5�   %   ��5�   %   �� $! (vc:CrudLookup.LookupWidth)�$ OneWay=� �� *TemplatedParent+ 5�   %   �� $ aPopup��$ (vc:AdornedPopup.IsShow)�$ TwoWay=� 5�   %   ��  �� $ TemplatedParent=�$ SelectedModel�$ TwoWay=�.��6�   .��5�   ;   5�      
  5�      $ aPopup��6.    6>   ��  �� *TemplatedParent+$ (vc:AdornedPopup.IsShow)�$ TwoWay=�.��5�   "   �� $ calDatePopupBorder��6)   #���  6E   $��0,1,0,0q�6s   $
��Auto��5�   -   #��z��6:   $
��Auto��6]   #��z��6k   ._�5�   &   �� $  DG_ScrollViewer��63   |�.  ��5�   *   �� 5�   .   " ��6>   .R�5�   2   � ! 5�   6   �� 5�   :   $
��Auto �6K   �� 5�   :   $��* �6K   �� 5�   :   $
��Auto �6K   " 5�   6   �� 5�   :   $
��Auto �6H   �� 5�   :   $��* �6H   �� 5�   :   $
��Auto �6H   .+�5�   6   �� #��Z# 6=   |�.  ��6~   C� *�� $"$ DataGridSelectAllButtonStyle��"%  +y�6�   �� *HeadersVisibility+#' Z& #) Z(  �� "*  ��6�  �� *CellsPanelHorizontalOffset+ �� "*    5�   6   $!+ PART_ColumnHeadersPresenter��6U   $��1��6z   y�6�   �� *HeadersVisibility+#' Z, #) Z(  �� "*  �� 5�   6   $!- PART_ScrollContentPresenter��6M   #. z��6r   $��2��6�   $��1��6�   �� 5�   6   $/ PART_VerticalScrollBar��6@   $��2��6`   #1 z0 6p   $2 Vertical=�6�   $��1��6�   #y�z3 6�   #5 z4 6c  6 6  �� *VerticalOffset+$ OneWay=� �� *TemplatedParent+6c  � 5�   6   $��1��6;   $��2��6K   ! 5�   :   �� 5�   >   ��6O   �� **(NonFrozenColumnsViewportHorizontalOffset+ �� "*  �� 5�   >   $��* �6O   .+�5�   :   �� $/ PART_HorizontalScrollBar��6D   $��1��6f   #1 z7 6v   $2 
Horizontal=�6�   #y�z8 6�   #5 z9 6a  6 6   �� *HorizontalOffset+$ OneWay=� �� *TemplatedParent+6a  5�   7   5�   3   5�   /   .��5�   *   �� # z 69   5�   '   5�   #   5�      5�      �� 5�      $: 
Lookup_ppp��6!   .+�5�        $; PART_FormEditCC��6(   $< 26��6A   #��z= 5�   +   #��z> 6X   $"? Редактирование��5�   +   $@ Сохранить��6I   $A 
Сброс��6b    5�   +   ��  �� $ TemplatedParent=�$ SelectedModel�8)  B 5�   +   ��  �� $ FindAncestor=�$C 2��$* 	vc:CrudCCL�D 5�   "   ���� 5�   &   '��       "  ��6,   .�5�   *   ��  � �� 6E   C� *�� "% ��$$ InThemeButtonBaseTemplate��+ 5�   *   �� 5�   .    6:   �� $ 	IsEditing� True6]   .a�5�   2   ��  � ��
 �  ��6O   5�   /   5�   '   5�   #   .��5�   "   �� #��zE 63   5�        5�      $; PART_FormNewCC��6(   $< 26��6@   #��zF 5�   +   #��zG 6W   $? Новая запись��5�   +   $@ Сохранить��6G   $A 
Сброс��6`    5�   +   ��  �� $ TemplatedParent=�$ SelectedModel�8)  $ OneWayToSource=�B 5�   +   ��  �� $ FindAncestor=�$C 2��$* 	vc:CrudCCL�D 5�   "   ���� 5�   &   '��       "  ��6,   .�5�   *   ��  � �� 6E   C� *�� "% ��$$ InThemeButtonBaseTemplate��+ 5�   *   �� 5�   .    6:   �� $ 	IsEditing� True6]   .a�5�   2   ��  � ��
 �  ��6O   5�   /   5�   '   5�   #   .��5�   "   �� $��FormNew��63   5�        5�      $H PART_FilterBlockCC��6/   .��5�   "   �� $��Filter��63   5�      5�      5�      	 5      P� 5     $I aPopup��6B   
 �@
 IsShow .  . �5     ��  �@ 
IsBindable .  6E   5     P� 5     
 �@=�HasError . . �5     �� $ PART_LookupBox��6%    6`   ��  �� *TemplatedParent+$ (Validation.Errors)[0]� �@
 Err5     5     5
     5     �� 5     "   6   .�5  
   ��  �@ AutoGenerateColumns .  60   �� 5  
    � ��$
 Left=�60   �� 5  
    � ��$	 Top=�6.   �� 5  
    �@ HeadersVisibility$ Column=�6.   �� 5  
    �@ RowHeaderStyle8   6+   �� 5  
    �@ ColumnHeaderStyle8  6.   �� 5  
    �@ 	CellStyle8  6&   �� 5  
    � ��8  6/   �� 5  
    �@��AlternationCount$ 2��6-   �� 5  
    �@ GridLinesVisibility$ Vertical=�60   �� 5  
    �@ SelectionMode$ Extended=�6*   �� 5  
    �@ ClipboardCopyMode$ ExcludeHeader=�6.   �� 5  
    �@ VerticalGridLinesBrush �#C6D7C563   �� 5  
    �@ SelectionUnit$ FullRow=�6*   �� 5  
    �@$�IsVirtualizing . 6B   �� 5  
   " �@ EnableColumnVirtualization .  67   �� 5  
    �@ EnableRowVirtualization . 64   �� 5  
    � �� . 6:   �� 5   
    �@=�ErrorTemplate 65   Z� �� 5!  
    � �� 5"     �� 5#     "  6"   Crud��.R�5$     � .+�5%     �� .+�5&     �� $J PART_LookupBox��6&   $
��Left=�5'  %   #���  5(  %   # z 5*  %   # �  5,  %   ��5)  %   �� $! (vc:CrudLookup.LookupWidth)�$ OneWay=� �� *TemplatedParent+ 5+  %   �� $ aPopup��$ (vc:AdornedPopup.IsShow)�$ TwoWay=� 5-  %   ��  �� $ TemplatedParent=�$ SelectedModel�$ TwoWay=�
  5.     $ aPopup��6.    6>   ��  �� *TemplatedParent+$ (vc:AdornedPopup.IsShow)�$ TwoWay=�.��5/  "   �� $ calDatePopupBorder��6)   #���  6E   $��0,1,0,0q�6s   $
��Auto��50  -   #��z��6:   $
��Auto��6]   #��z��6k   ._�51  &   �� $  DG_ScrollViewer��63   |�.  ��52  *   �� 53  .   " ��6>   .R�54  2   � ! 55  6   �� 56  :   $
��Auto �6K   �� 57  :   $��* �6K   �� 58  :   $
��Auto �6K   " 5:  6   �� 5;  :   $
��Auto �6H   �� 5<  :   $��* �6H   �� 5=  :   $
��Auto �6H   .+�5?  6   �� #��Z# 6=   |�.  ��6~   C� *�� $"$ DataGridSelectAllButtonStyle��"%  +y�6�   �� *HeadersVisibility+#' Z& #) Z(  �� "*  ��6�  �� *CellsPanelHorizontalOffset+ �� "*    5@  6   $!+ PART_ColumnHeadersPresenter��6U   $��1��6z   y�6�   �� *HeadersVisibility+#' Z, #) Z(  �� "*  �� 5A  6   $!- PART_ScrollContentPresenter��6M   #. z��6r   $��2��6�   $��1��6�   �� 5B  6   $/ PART_VerticalScrollBar��6@   $��2��6`   #1 z0 6p   $2 Vertical=�6�   $��1��6�   #y�z3 6�   #5 z4 6c  6 6  �� *VerticalOffset+$ OneWay=� �� *TemplatedParent+6c  � 5C  6   $��1��6;   $��2��6K   ! 5D  :   �� 5E  >   ��6O   �� **(NonFrozenColumnsViewportHorizontalOffset+ �� "*  �� 5F  >   $��* �6O   .+�5H  :   �� $/ PART_HorizontalScrollBar��6D   $��1��6f   #1 z7 6v   $2 
Horizontal=�6�   #y�z8 6�   #5 z9 6a  6 6   �� *HorizontalOffset+$ OneWay=� �� *TemplatedParent+6a  5I  7   5J  3   5K  /   .��5M  *   �� # z 69   5N  '   5O  #   5P     5Q       5R     $H PART_FilterBlockCC��6+   .��5S     �� 5T     5U     	 5V     P� 5W     $I aPopup��6B   
 �@
 IsShow .  . �5X     ��  �@ 
IsBindable .  6E   5Y     P� 5Z     
 �@=�HasError . . �5[     �� $ PART_LookupBox��6%    6`   ��  �� *TemplatedParent+$ (Validation.Errors)[0]� �@
 Err5\     5^     5`     5a     5b     