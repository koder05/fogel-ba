   M S B A M L   `   `   `  ���� S  ORF.WinApp.Infrastructure, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullG9clr-namespace:RF.WinApp;assembly=RF.WinApp.Infrastructure	RF.WinApp  D @RF.WinApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:RF.WinApp	RF.WinApp W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089   ArrowContinue  � Points  ArrowEnd  MouseOverListBrush �� 
StartPoint �� EndPoint � Offset � Color  SelectedItemListBrush �� Color �� Opacity  HalfOpacityBrush  ColumnHeader �� 
TargetType=   7System.Windows.Controls.Primitives.DataGridColumnHeader �� Property	 �� Value
 �� 
TargetType � 	Resources  Key �� ImageSource �� ContentStringFormat �� ContentStringFormat  SortPathStyle E� 
StartPoint �� Point �� Triggers P� Property P� Value �� 
TargetName  	RowHeader:  4System.Windows.Controls.Primitives.DataGridRowHeader 	 SelectPathStyle :� RoutedEvent �� TargetProperty g� Duration	 W� To W� From g� 	BeginTime �� Value �� Binding �� RelativeSource �� AncestorType)  #System.Windows.Controls.DataGridRow �� Path 
 	CellStyle*  $System.Windows.Controls.DataGridCell  J� SnapsToDevicePixels! �� Triggers  	ItemStyle  GridToolBarStyle   RF.WinApp.FilterCC  ToolBarButtonStyle" ��Name   RF.WinApp.ActionBlock  ActionBlockStyle-  'System.Windows.Input.KeyboardNavigation# �� BasedOn   RF.WinApp.FormCC   RF.WinApp.FilterBlockCC	   RF.WinApp.CrudCC&
   System.Windows.Controls.DataGrid$ ��Name% � RowDefinitions& � ColumnDefinitions' 
  SelectAllCommand( �� 
ResourceId) �� TypeInTargetAssembly7  1System.Windows.Controls.DataGridHeadersVisibility
*   All+ �� ConverterParameter!, 
  HeadersVisibilityConverter- �� 	ConverterG  ASystem.Windows.Controls.Primitives.DataGridColumnHeadersPresenter.  Name/   Column0 ��Name1 �� CanContentScroll2 ��Name3 �� ScrollableHeight4 � Maximum5 �� Orientation*6 �� #ComputedVerticalScrollBarVisibility7 �� ViewportHeight8 �� ViewportSize9 � Value: �� Mode; �� ScrollableWidth,< �� %ComputedHorizontalScrollBarVisibility= �� ViewportWidth>  Name? 	  FormEditBlockWidth@ 	  FormEditBlockHeightA   BlockCaptionB   ApplyCaptionC   ClearCaptionD � DataContextE �� ModeF � 	Resources  applybtnG 	  FormEditH 	  FormNewBlockWidthI 	  FormNewBlockHeightJ  NameK 	  FilterBlockWidthL 	  FilterBlockHeightM �� OrientationN �� LeftO �� RightP J� Opacity  ToolBarButtonBaseStyleQ �� ElementNameR 	  MoveFirstActionS � ToolTipT � LayoutTransformU �� AngleV 	  MoveBackwardActionW ��NameX 	  MoveForwardActionY 	  MoveLastActionZ 	  NewRowAction  FormNewButton
[ �� Top\ 	  DeleteAction] ��Name^ 	  ToggleFilterAction  FilterButton_ � UseLayoutRounding` 	  ToggleFormAction  UFormButton   RF.WinApp.ContentCC # gridSplitterDotsBackgroundBrush  gridSplitterStyle   RF.WinApp.PlatesCCa �Nameb � ShowGridLinesc A�Named A� Columnse A� Rowsf ��Nameg �� ResizeBehavior   RF.WinApp.GifImageh  Namei   	GifSourcej   	AutoStart   RF.WinApp.ScaleBoxCCk �� Paddingl �� CornerRadiusm ]�Namen   CloseActiono ��Namep (�Name   RF.WinApp.SplitButtonq �� ClearTypeHint  SubMenuBackgroundBrushr   DirectionalNavigations   ApplyActiont �Nameu � AllowsTransparencyv � HorizontalOffsetw � VerticalOffsetx   TabNavigationy � IsSharedSizeScopez ��Name{ �� ContextMenu| ��Name} P� 
SourceName~ K� ZIndex K� IsItemsHost� F�Name  ItemEdge  ItemEdgeBorder   RF.WinApp.TabsPlatesCC�   ItemCloseAction� �� NotifyOnSourceUpdated� �� UpdateSourceTrigger� �� ScaleX� ��Name�� D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    :x,http://schemas.microsoft.com/winfx/2006/xaml     Clocal9clr-namespace:RF.WinApp;assembly=RF.WinApp.Infrastructure  "local2clr-namespace:RF.WinApp  %�B  &	       &	 .    &	 �     &	     &	 �    &	 �    7  7  &	 �    7 	 &	
 �	    7  &	 �    7  &	 �    7  '  &    &	 N    &	 y    '  �    7  '  �    7  '  �    7  '	  �    7  7  7 
 7  7  7  7  7  7  7  7  7  7  7  7  7   7   7  7  7   7  7  '  �%    &	 �%    &	 &    7  7  7  7  7  '  �(    7  '  G+    7  7  '  �.    7  7  '�� 8    '�� q9    7  7  7  7  7  '  �?    �   �   ��[  	=  z 
���   �� G  �	   ��[  	= ��[ ��j ��j  z ��[  z ��[  	=  z 
���   ��� $	 0,0&�$	 0,1&���� $	 0.0Q�$ #FAE9C4��� $	 0.5Q�$ #F9E5BA��� $
 0.51Q�$ #F7DA9F��� $	 1.0Q�$ #F4D9A3���� $	 0,0&�$	 0,1&���� $	 0.0Q�$ #F3C667��� $	 0.5Q�$ #F0BA4C��� $
 0.51Q�$ #EAA008��� $	 1.0Q�$ #E39E11���� $ 	#FF688CAF��$ .3Q��� "   .���  � ��$	 Center=���  � ��	 �#656A64��  � ��$	 35����  � ��$	 Arial)���  � ��$	 12(���  � ��	 �� "
    ���� 'n�       " n�.���  � ��$	 1����  �@�Cursor$	 SizeWE����  � ��	 �� "
 n�.R��� #��z���� SortPathStyle " F�.���  � ��
	 � � ���  � ��$	 Right=���  � ��$	 Center=���  � ��$	 0,2,7,0q���  � y�$	 Hidden=�.R�� .+��� $��
MainBorder������ $F @/RF.WinApp;component/Img/modules_view_grid_header_background.png��._�� .+��� $��3q�$��Center=�$��Center=�#��z��# z #��z��F� $��UpArrow��8��  ��B� .��E� $	 0,0&�.���� $	 7,0&��� $ 3.5,6&��� $	 0,0&�F� $��	DownArrow��8�� ��B� .��E� $	 0,6&�.���� $	 7,6&��� $ 3.5,0&��� $	 0,6&�n� $��PART_LeftHeaderGripper��$
��Left=����#F7FEF7n� $��PART_RightHeaderGripper��$��Right=����#C6D7C5 P�  �@  SortDirection$ 
DescendingX�. ��� $ UpArrow�� � y�$	 Hidden=��� $ 	DownArrow�� � y�$	 Visible=�P�  �@  SortDirection$ 	AscendingX�. ��� $ 	DownArrow�� � y�$	 Hidden=��� $ UpArrow�� � y�$	 Visible=��� "  .���  � ��$	 20����  � ��	 �� "
   ���� 'n�       " n�.���  � ��$	 1����  � ��	 �� "
 n�.R��� #��z���� SelectPathStyle " F�.���  � ��
	 � � ���  � ��$	 Center=���  � ��$	 Center=���  � y�$	 Hidden=�.R�� .+��� $��
MainBorder������ $F @/RF.WinApp;component/Img/modules_view_grid_header_background.png��F� $��Arrow��8��  ��B� .��E� $	 0,0&�.���� $	 6,4&��� $	 0,8&��� $	 0,0&� P�  �@ IsRowSelected . . ��� $ Arrow�� � y�$	 Visible=�:� $ Binding.SourceUpdated��.[��� .���� .��W� $ Width�$ 0:0:0D�$ 50X�W� $ Width�$ 0:0:2D�$ 50X�$ 20X�$ 0:0:0X��� $
 True�� ��  �� *FindAncestor+"  $ Model.IsEditing�.a��� $ Arrow�� � ��
	 �  ���� "  .���  � ��$	 Arial)���  � ��$	 12(���  � ��$	 CharacterEllipsis=���  � ��
	 �   ���  � ��$	 0q���  � ��$	 20����  � ��	 �� "
  .R��� #��z��#��z��#��z��  . ._��� $��Center=�#��z��$��1,0,0,0q�#��z��# z #  z  ! P�  �@ 
IsSelected . . ���  � ��8	   ��  � ��
	 �   ���  � ��
	 ���� P�  �@J�	IsFocused . . ���  � ��	 �#FEECC8��  �� $ ID� 0.a���  � ��
	 �������  � ��
	 ������� "  .���  � ��$	 0,0,0,0q�! P�  �@ AlternationIndex$ 0��. ���  � ��
	 �����P�  �@ AlternationIndex$ 1��. ���  � ��	 �#EFF3EFP�  �@J�IsMouseOver . . ���  � ��8	   �� " ��.���  � ��8	   ��  �@��Orientation$	 
Horizontal=��� "  .���  � ��	 �� "
  �� " ��.���  �@J�SnapsToDevicePixels	 . ��  �@�OverridesDefaultStyle	 . ��  � ��$	 0,0,0,0q���  � ��$	 20����  � ��$	 20����  � ��	 �� "
 ��.R��� $" Border��$��0q�
������ 
������ ._��� $��0,0,0,0q�$��Center=�$��Center=���. �� "  .���  �@ TabNavigation$	 	Contained=���  � y�$	 Hidden=��� "  8#   �� "  8#   �� "  8#   �� " 	 .���  �@J�SnapsToDevicePixels	 . ��  �@
 AutoGenerateColumns	 .  ��  � ��
	 �������  � ��	 �	#FF688CAF��  � ��$	 0,0,0,1q���  � ��$	 Stretch=���  � ��$	 Stretch=���  �@
 ColumnHeaderStyle8	   ��  �@
 RowHeaderStyle8	  ��  �@
 	CellStyle8	  ��  �@
 
IsReadOnly	 . ��  �@
 HeadersVisibility$		 All=���  �@��AlternationCount$	 2����  � ��8	  ��  �@
 GridLinesVisibility$	 Vertical=���  �@
 SelectionMode$	 Extended=���  �@
 ClipboardCopyMode$	 ExcludeHeader=���  �@
 VerticalGridLinesBrush	 �#C6D7C5��  �@
 SelectionUnit$	 FullRow=���  �@$�IsVirtualizing	 . �� " �@
 EnableColumnVirtualization	 .  ��  �@
 EnableRowVirtualization	 . ��  � ��	 . ��  � ��	 �� "
 	  ���� '��       " ��.���  �@J�SnapsToDevicePixels	 . ��  �@�OverridesDefaultStyle	 .  ��  � ��$	 0,0,0,0q���  � ��$	 18����  � ��$		 110����  � ��	 �� "
 ��.R��� $" Border��$��1q�
�������
�������._��� $$ PART_ContentHost��$��0q�.R��� $��panel1��$��1q����	#FF688CAF$��Stretch=�$��Stretch=�._�� % �� $��* ��� $��26 �.+��� $$ DG_ScrollViewer��|�.  $��0������ "
 ��.R�� & �� $
��Auto ��� $��* ��� $
��Auto �% �� $
��Auto ��� $��* ��� $
��Auto �.+��� #��Z' |�.  ��C� *�� $"( DataGridSelectAllButtonStyle��") 
 +y��� *HeadersVisibility+#+ Z* #- Z,  �� " 
 ���� *CellsPanelHorizontalOffset+ �� " 
   $!. PART_ColumnHeadersPresenter��$��1��y��� *HeadersVisibility+#+ Z/ #- Z,  �� " 
 �� $!0 PART_ScrollContentPresenter��#1 z��$��2��$��1���� $2 PART_VerticalScrollBar��$��2��#4 z3 $5 Vertical=�$��1��#y�z6 #8 z7 9 �� *VerticalOffset+$: OneWay=� �� *TemplatedParent+� $��1��$��2��& �� ���� **(NonFrozenColumnsViewportHorizontalOffset+ �� " 
 �� $��* �.+��� $2 PART_HorizontalScrollBar��$��1��#4 z; $5 
Horizontal=�#y�z< #8 z= 9 �� *HorizontalOffset+$: OneWay=� �� *TemplatedParent+.���� #  z    $> PART_FormEditCC��$��0��8�� #��z? #��z@ $y�Hidden=�$"A Редактирование��$B Сохранить��$C 
Сброс��D ��  �� $E FindAncestor=�" 
 $ SelectedItem�F ���� '��       " ��applybtn ! ��  �� $ 	IsEditing� True.a���  � ��
	 �  ���� '��       " ��8#  .���� #��zG   $> PART_FormNewCC��$��0��8�� #��zH #��zI $y�Hidden=�$A Новая запись��$B Сохранить��$C 
Сброс��F ���� '��       " ��applybtn ! ��  �� $ 	IsEditing� True.a���  � ��
	 �  ���� '��       " ��8#  .���� $��FormNew��  $J PART_FilterBlockCC��$��0��8�� #��zK #��zL $y�Hidden=�$A Фильтр��$B Применить��$C Очистить��D ��  �� $E FindAncestor=�" 
 $ SelectedItem�.���� $��Filter���� $��spNavigation��$��1��$��Stretch=����#FAE9C4$M 
Horizontal=�8��	 F ���� '��       " ��ToolBarButtonBaseStyle .���  �@J�SnapsToDevicePixels	 . ��  �@�OverridesDefaultStyle	 . ��  � ��$	 0,0,0,0q���  � ��$	 20����  � ��$	 20����  � ��
	 ���� ��  � ��	 �� "
 ��.R��� $" Border��$��0q�#��z��
������ ._��� .+��� $��0,0,0,0q�$��Center=�$��Center=���. #��z��#��z��$N 0��$O 0���� $" DisableCouple�����#777777$	P 0.5Q�$y�Hidden=�#��z��#��z��$N 0��$O 0�� P�  �@J�IsMouseOver . . ��� $ Border�� � ��	 �#E39E11P�  � {� .  . ��� $ DisableCouple�� � y�$	 Visible=��� 8# 
 '��       " ���� 8#  '��       " ��FilterButton ! ��  �� $Q PART_FilterBlockCC��$ Content.Content� Z� .a���  � {�	 .  ��  �� $Q PART_FilterBlockCC��$ IsOpened� True.a���  � ��	 �#E39E11�� 8#  '��       " ��FormNewButton ! ��  �� $Q PART_FormNewCC��$ Content.Content� Z� .a���  � {�	 .  ��  �� $Q PART_FormNewCC��$ IsOpened� True.a���  � ��	 �#E39E11�� 8#  '��       " ��UFormButton ! ��  �� $Q PART_FormNewCC��$ IsOpened� True.a���  � {�	 .  ��  �� $Q PART_FormEditCC��$ IsOpened� True.a���  � ��	 �#E39E11��  �� $Q PART_FormEditCC��$ Content.Content� Z� .a���  � {�	 .  .+��� $��Запись:��$��14��$��45��$��3,0,0,0q�$N 14���� #��ZR $S К началу��.���� $��Center=�$��Center=�8�� T �� $	U 180Q��� #��ZV $S 
Назад��.���� $��Center=�$��Center=�8�� T �� $	U 180Q��� $W PART_RowsTB���� #��ZX $S Вперед��.���� $��Center=�$��Center=�8�� �� #��ZY $S В конец��.���� $��Center=�$��Center=�8�� �� $��btnNew��$S Новая запись��#��ZZ 8�� .���� $��14��$��8��.+��� 8�� A� $��6��$��6��
���  ��$��1��
���  ��$N 8��$[ 1���� $��	btnDelete��$S Удалить��#��Z\ .���� $0��*/RF.WinApp;component/Img/delete-2-icon.png��$��14��$��14���� $��3,0,3,0q�T �� $U 90Q��� $] 	btnFilter��$��65��#��Z^ 8�� _ . .���� $��55��$��10��.+�� 9  �    � ��j @KL ��j ��� @KL ��� @KL @KL 
���   �$��1���� $��Фильтр��$N 14���� $��btnFormEdit��$��65��#��Z` 8�� $��2,0,0,0q�.���� $��55��$��10��.+��� $8��2/RF.WinApp;component/Img/application-form-icon.png��$��18��$��18��$[ -4��$N -2���� $��
Форма��$N 20���� "  .���  � ��	 �� "
  .R��� �� $	 0,0&�$	 0,1&���� $ 	#FFE3EFFF��$ 0Q�� $ 	#FFAFD2FF��$ 1Q��� " ��.���  � ��$	 8����  �@J�SnapsToDevicePixels	 . ��  � |�	 .  ��  � ��$	 7,7,7,7q���  �@�Cursor$	 SizeWE����  � ��	 �	#FF204D89��  � ��	 �� .R��� ._��� $��3��$��19��.+��� 8��  $��2��$��2��$[ 1��$N 0���� 8�� $��2��$��2��$[ 5��$N 0���� 8�� $��2��$��2��$[ 9��$N 0���� 8�� $��2��$��2��$[ 13��$N 0���� 8�� $��2��$��2��$[ 17��$N 0���� #��z��$��2��$��2��$[ 0��$N 0���� #��z��$��2��$��2��$[ 4��$N 0���� #��z��$��2��$��2��$[ 8��$N 0���� #��z��$��2��$��2��$[ 12��$N 0���� #��z��$��2��$��2��$[ 16��$N 0���� "  .���  � ��	 �� "
  .R�� $a PART_ItemsPanel��$��Stretch=�$��Stretch=�b .  % �� $��* �& �� $��1 ��� $��* �.+�A� $c PART_WidgetsPanel��$d 1��$e 1��$��Stretch=�$��Stretch=�$��0��$��0,0,7,0q��� $f gridSplitterTreeNodes��8��  $��0��$��Right=�$��Stretch=�$g BasedOnAlignment=��� $" PART_MainPlaceHolder��$��1��$��Stretch=�$��Stretch=��� $" PART_WaitOverdoor��$��1�����#eeeeee$	P 0.2Q�$��Stretch=�$��Stretch=�$y�	Collapsed=�  $h PART_GifWait��$
��None=�$��Center=�$��Center=�$-i '/RF.WinApp;component/Img/bar-circle.gif��j . $��1��$y�	Collapsed=��� "  .���  � ��	 �� "
  .R��� ���#F9F9F9
�������$��2q�$k 5,5,5,5q�$��5,5,5,5q�$l 8,8,8,8��._�� % �� $��20 ��� .+�]� $m TabItemContent����.  .+��� 8��  $��Right=�$��Center=�$��Right=�#��Zn .���� $(��"/RF.WinApp;component/Img/close.png��$��16��$��16���� $o Title��$
��Left=�$
��Left=�$��Center=�
�������$��2,0,0,1q����� $ (local:ViewName.Title)� �� *TemplatedParent+�� 8�� $��0��#��Zn $��Right=�.���� $(��"/RF.WinApp;component/Img/close.png��$��16��$��16��(� $p PART_Viewbox��$��Stretch=�$	��Top=�$��1���� $" Overdoor��$��0��$��2�����#6F6F90$	P 0.1Q�$��Stretch=�$��Stretch=� P�  �@J�IsMouseOver . . ��� $ Overdoor�� � y�$	 	Collapsed=��� $ Title�� � ��
	 �   ��� "  .���  � ��$	 0,1,0,1q���  � ��$	 4,1,5,2q���  � ��	 �� "
   ���� '��       " ��.���  �@J�SnapsToDevicePixels	 . ��  �@�OverridesDefaultStyle	 . ��  � ��$	 0,0,0,0q���  � ��	 �� "
 ��.R��� $" Border��$��1q�
������ #��z��._��� $��0,0,0,0q�$��Center=�$��Center=���. �� SubMenuBackgroundBrush $ 	#FFF5F5F5���� '��       " ��.���  � ��$	 Center=���  �@�IsSharedSizeScope	 . ��  �@��PanningMode$
	 Both=���  �@��IsFlicksEnabled	 .  ��  � ��	 �� "
 ��.R��� $��Border��$��1q�
������ 
�������._��� $��ContextMenuScrollViewer����. ��C� *�� ") �$( MenuScrollViewer��+.��� $q Enabled=�.+��� $��0��$��0��$
��Left=�$	��Top=�.+��� 8��  ���� $Q Border��$ ActualHeight����� $Q Border��$ ActualWidth��� $��ItemsPresenter��$��2q�$r Cycle=�#  z  .R��� $] Chrome��#��z��
������ #��Zs .��� & �� �� $
��Auto ��� $
��Auto �.+��� #��z��$��Center=�$��2,2,4,2q���. $��Center=�� $t 
PART_Popup��u . $��Bottom=�$v -1��$w 2��|�.  #��� -����� $ IsSubmenuOpen� �� *TemplatedParent+.���� $��1q�
������ 
�������._��� $��SubMenuBorder����.  .���� $��SubMenuScrollViewer����. ��C� *�� ") �$( MenuScrollViewer��+.��� $q Enabled=�.+��� $��0��$��0��$
��Left=�$	��Top=�.+��� 8�� ���� $Q SubMenuBorder��$ ActualHeight����� $Q SubMenuBorder��$ ActualWidth��� $��ItemsPresenter��$��2q�$x Cycle=�$r Cycle=�#  z  y . �� $z Splitter��$��1��$��1��#��z��$	P 0.4Q�$��0 2 0 2q�� $a SplitElement��$��2��
������ { �� $| SubMenu��#��z��#��z������ *ButtonMenuItemsSource+ �� *TemplatedParent+.+�F� "����        � z     � 	=  	= 	#��z��$��2 0 4 0q�$��Center=� P�  � {� .  . ���  � ��	 �	#FFADADADP�  �@m�IsHighlighted . . ��� $ Chrome�� � ��
	 �����P�  �@J�IsKeyboardFocused . . ��� $ Chrome�� � ��
	 �����P� $} SubMenu�� �@��IsOpen . . ��� $ Chrome�� � ��
	 �����P�  �@m�IsSubmenuOpen . . ��� $ Chrome�� � ��
	 �����P� $} SubMenu�� �@��HasItems .  . ��� $ SplitElement�� � y�$	 	Collapsed=��� $ Splitter�� � y�$	 	Collapsed=��� " ��.���  � ��	 �� "
 ��.R�� % �� $
��Auto ��� $��* �.+��� $��2,0,2,2q�$~ 2�����#aaaaaa$��1��._��� ���#ffffff$��5,10,5,5q�
�������._��� $��SelectedContent���� #��� ��$��Stretch=�$��0,0,0,2q����#aaaaaa$~ 1��._�A� $c 	ItemsHost�� . $
��Left=�$��0q�$e 1��d �� $ Children.Count��� " ��.���  �@J�SnapsToDevicePixels	 . ��  � ��	 �� "
 �� ���� ItemEdge " F�.���  � ��U	 ��     6n� 	=  -1 @KL @KL р�[  	= ׀��     �@KL @KL р��     р��  6n	��  � ��	 �#FAE9C4��  � ��$	 Bottom=��� ItemEdgeBorder " F�.���  � ��L	 ��    ���� 	=  -1 @KL @KL р�[  	= ׀��     �@KL @KL р��     	��  � ��$	 2����  � ��	 �#aaaaaa��  � ��$	 Bottom=���  � ��$	 0,0,0,-2q�.R��� $" TabItemBorder��$��
0,0,-10,-2q�$��0,0,0,2q����#aaaaaa$��Bottom=�._�� $	a grd��& �� $��14 ��� $��* �$	��250���� $��14 �.+�F� $� 	TabPath12��8��  $��0��F� $� 	TabPath11��8�� $��0���� $" TabItemContentBorder��$��1��$��0,2,0,0q����#aaaaaa���#FAE9C4$��Bottom=�._�]� $m TabItemContent����.  $��24��.+��� 8�� $��Right=�$��Center=�$��Right=�#��Z� ���� $ Content� �� *TemplatedParent+.���� $(��"/RF.WinApp;component/Img/close.png��$��16��$��16���� $
��Left=�$
��Left=�$��Center=����� $ (local:ViewName.Title)�� . $� PropertyChanged=�F� $� 	TabPath22��8�� $��2��T �� $� -1Q�F� $� 	TabPath21��8�� $��2��T �� $� -1Q� P� $	} grd�� �@J�IsMouseOver . P�  �@��
IsSelected . . ��� $ 	TabPath12�� � ��	 �	#FFFFFFFF�� $ 	TabPath22�� � ��	 �	#FFFFFFFF�� $ TabItemContent�� � ��	 �	#FFFFFFFF�� $ TabItemBorder�� � ��	 �	#FFFFFFFF��  �@K�ZIndex$	 2���� "  .���  � ��	 �� "
  .R�� #��� ��.+��� $� 	PART_Tabs��$��2,0,2,0q��� $" PART_WaitOverdoor�����#eeeeee$	P 0.2Q�$��Stretch=�$��Stretch=�$y�	Collapsed=�$��2,0,2,0q�  $h PART_GifWait��$
��None=�$��Center=�$��Center=�$-i '/RF.WinApp;component/Img/bar-circle.gif��j . $��1��$y�	Collapsed=�