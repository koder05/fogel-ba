   M S B A M L   `   `   `  ����   	RF.WinApp    RF.WinApp.Views.HomeMenu   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008        ThemeMenuItemStyle#���   $��	Home Menu������ *MenuObjects+  � 	Resources    HighlightedBackgroundBrush �� Color  MenuBackgroundBrush  NormalBorderBrush  SolidMenuFontBrush  HighlightedText �� 
TargetType �� Property �� Value �� 
TargetType ��Name � ColumnDefinitions ��Name	 z� SharedSizeGroup
 ��Name �Name F�Name �Name �� Path �� RelativeSource �� Padding �� AncestorType � IsSharedSizeScope K� IsItemsHost-  'System.Windows.Input.KeyboardNavigation   DirectionalNavigation �� Triggers P� Property P� Value �� 
TargetName��%`
  &	       &	      &	 *     &	 ?     &	 T     &	  i     7  7  7  7  7  7  �� $ 	#FF797878���� $ 	#FF505050���� $ 	#FFFFFFFF���� $ 	#FFFFFFFF���� $ 	#FFFFFFFF���� " m�.���  � �� �� *Name+��  �@m�Command �� *ActivateCommand+��  �@m�CommandParameter �� *View+��  � �� �� " m�.R��� $ Border��#��z��#��z��$��1q�._��  �� $
 Col0��$��17��$
��Auto �$	 MenuItemIconColumnGroup���� $
��Auto �$	 MenuTextColumnGroup���� $
��Auto �$	 MenuItemIGTColumnGroup���� $
 Col3��$��14 �.+��� $

 Icon��$��0��$��4,0,6,0q�$��Center=�$
��Icon��� $ 
GlyphPanel��$��0��$y�Hidden=�$��4,0,6,0q�$��Center=�.+�F� $ GlyphPanelpath��$��Center=�#��z��=����    �� �     >I Ѡ%& @�p �`Vl ��* �`Vl     Ѡ%& �0F 	$��LeftToRight=��� $
 
HeaderHost��$��1��#��z����. $��Header���� $
 IGTHost��$��2��$��8,1,8,1q�$��InputGestureText��$��Center=�� $ 
ArrowPanel��$��3��$��4,0,6,0q�$��Center=�.+�F� $ ArrowPanelPath��$��Right=�$��Center=�#��z��"����        �     z � 	=  	= 	� $ SubMenuPopup��$��Right=�|�.  #��� -����� $ IsSubmenuOpen� �� *TemplatedParent+.���� $ SubMenuBorder��$��1q�$ 2,2,2,2q����� $ 
Foreground� �� " o�._�� $ SubMenu�� . .+���  . $ Cycle=� P�  �@m�Role$ TopLevelHeader=�. ���  � ��$ 6,1,6,1q��� $ SubMenuPopup�� � ��$ Bottom=��� $
 Col0�� � ��$ 0���� $
 Col3�� � ��$
 Auto ��� $
 Icon�� � y�$ 	Collapsed=��� $ 
GlyphPanel�� � y�$ 	Collapsed=��� $ IGTHost�� � y�$ 	Collapsed=��� $ 
ArrowPanel�� � y�$ 	Collapsed=�P�  �@m�Role$ TopLevelItem=�. ���  � ��$ 6,1,6,1q��� $ 
ArrowPanel�� � y�$ 	Collapsed=�P�  �@m�Role$ SubmenuHeader=�. ���  � ��$	 Top=���  � ��$ 0,2,0,2q�P�  �@m�Role$ SubmenuItem=�. ���  � ��$	 Top=���  � ��$ 0,2,0,2q��� $ 
ArrowPanel�� � y�$ 	Collapsed=�P� " �@m�IsSuspendingPopupAnimation . . ��� $ SubMenuPopup�� � ��$
 None=�P�  Z�  �@m�Icon. ��� $
 Icon�� � y�$ 	Collapsed=�P�  �@m�	IsChecked . . ��� $ 
GlyphPanel�� � y�$ Visible=��� $
 Icon�� � y�$ 	Collapsed=�P�  �@m�IsHighlighted . . ��� 8   $ Border�� � ���� 8   � ���� 8  $ Border�� � ��P�  �@m�IsHighlighted .  . ��� 8  $ Border�� � ���� 8   � ���� 8  $ Border�� � ��P�  � {� .  . ���  � ��
 �����