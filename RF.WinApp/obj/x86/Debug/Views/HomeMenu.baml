   M S B A M L   `   `   `  ����  	RF.WinApp    RF.WinApp.Views.HomeMenu   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5         ThemeMenuItemStyle#���   5      $��	Home Menu��5	      ��5      �� *MenuObjects+  � 	Resources  5        HighlightedBackgroundBrush �� Color  MenuBackgroundBrush  NormalBorderBrush  SolidMenuFontBrush  HighlightedText �� 
TargetType �� Property �� Value �� 
TargetType ��Name � ColumnDefinitions ��Name	 z� SharedSizeGroup
 ��Name �Name F�Name �Name �� Path �� RelativeSource �� Padding �� AncestorType � IsSharedSizeScope K� IsItemsHost-  'System.Windows.Input.KeyboardNavigation   DirectionalNavigation �� Triggers P� Property P� Value �� 
TargetName��%  &	       &	 #     &	 F     &	 i     &	 �     &	  �     7  7  7  7  7  7  �� 5   
   $ 	#FF797878��6=   �� 5   
   $ 	#FF505050��66   �� 5   
   $ 	#FFFFFFFF��64   �� 5   
   $ 	#FFFFFFFF��65   �� 5   
   $ 	#FFFFFFFF��62   �� 5   
   " m�6+   .�5      ��  � �� 6'   �� *Name+6@   �� 5       �@m�Command 6(   �� *ActivateCommand+�� 5       �@m�CommandParameter 61   �� *View+�� 5       � �� 5      �� 5      " m�6&   .R�5      �� $ Border��6!   #��z��5      #��z��5      $��1q�5      ._�5      �  5   "   �� 5   &   $
 Col0��67   $��17��6E   $
��Auto �6S   $	 MenuItemIconColumnGroup��6`   �� 5   &   $
��Auto �67   $	 MenuTextColumnGroup��6D   �� 5    &   $
��Auto �67   $	 MenuItemIGTColumnGroup��6D   �� 5!   &   $
 Col3��67   $��14 �6E   .+�5%   "   �� $

 Icon��6T   $��0��63   $��4,0,6,0q�6C   $��Center=�6b   $
��Icon��6}   � 5(   "   $ 
GlyphPanel��6\   $��0��6'   $y�Hidden=�67   $��4,0,6,0q�6K   $��Center=�6p   .+�5)   &   F� $ GlyphPanelpath��6+   $��Center=�6C   #��z��6^   =����    �� �     >I Ѡ%& @�p �`Vl ��* �`Vl     Ѡ%& �0F 	$��LeftToRight=�6�   5*   #   �� 5-   "   $
 
HeaderHost��5/   !   $��1��5-   3   #��z��5.   !   ��. $��Header��51   !   �� 54   "   $
 IGTHost��56   !   $��2��54   3   $��8,1,8,1q�55   !   $��InputGestureText��57   !   $��Center=�58   !   � 5;   "   $ 
ArrowPanel��6H   $��3��6'   $��4,0,6,0q�67   $��Center=�6\   .+�5<   &   F� $ ArrowPanelPath��6+   $��Right=�6C   $��Center=�6_   #��z��6z   "����        �     z � 	=  	= 	6�   5=   #   � 5@   "   $ SubMenuPopup��6�   $��Right=�6   |�.  #��� -�6�   ��6(   �� $ IsSubmenuOpen� �� *TemplatedParent+.��5A   &   �� $ SubMenuBorder��6-   $��1q�6�   $ 2,2,2,2q�6�   ��6D   �� $ 
Foreground� �� " o�._�5B   *   � $ SubMenu��6/    . .+�5D   .   ��  . $ Cycle=�6L   5E   +   5F   '   5G   #   5H      5I       5L      P� 5N       �@m�Role$ TopLevelHeader=�66   . �5O   "   ��  � ��$ 6,1,6,1q�6<   �� 5P   "   $ SubMenuPopup��6M    � ��$ Bottom=�6>   6M   �� 5Q   "   $
 Col0��6G    � ��$ 0��6=   6G   �� 5R   "   $
 Col3��6G    � ��$
 Auto �6:   6G   �� 5S   "   $
 Icon��6Q    � y�$ 	Collapsed=�6?   6Q   �� 5T   "   $ 
GlyphPanel��6Q    � y�$ 	Collapsed=�6?   6Q   �� 5U   "   $ IGTHost��6Q    � y�$ 	Collapsed=�6?   6Q   �� 5V   "   $ 
ArrowPanel��6Q    � y�$ 	Collapsed=�6?   6Q   5W      P� 5Z       �@m�Role$ TopLevelItem=�66   . �5[   "   ��  � ��$ 6,1,6,1q�6<   �� 5\   "   $ 
ArrowPanel��6Q    � y�$ 	Collapsed=�6?   6Q   5]      P� 5`       �@m�Role$ SubmenuHeader=�66   . �5a   "   ��  � ��$	 Top=�6C   �� 5b   "    � ��$ 0,2,0,2q�6<   5c      P� 5f       �@m�Role$ SubmenuItem=�66   . �5g   "   ��  � ��$	 Top=�6C   �� 5h   "    � ��$ 0,2,0,2q�6<   �� 5i   "   $ 
ArrowPanel��6Q    � y�$ 	Collapsed=�6?   6Q   5j      P� 5k      " �@m�IsSuspendingPopupAnimation . . �5l   "   �� $ SubMenuPopup��6P    � ��$
 None=�6C   6P   5m      P� 5p       66   Z�  �@m�Icon. �5q   "   �� $
 Icon��6Q    � y�$ 	Collapsed=�6?   6Q   5r      P� 5u       �@m�	IsChecked . . �5v   "   �� $ 
GlyphPanel��6O    � y�$ Visible=�6?   6O   �� 5w   "   $
 Icon��6Q    � y�$ 	Collapsed=�6?   6Q   5x      P� 5{       �@m�IsHighlighted . . �5|   "   �� 8   $ Border��6s    � ���� 5}   "   8   � ��6?   �� 5~   "   8  $ Border��6k    � ��5      P� 5�       �@m�IsHighlighted .  . �5�   "   �� 8  $ Border��6l    � ���� 5�   "   8   � ��6?   �� 5�   "   8  $ Border��6m    � ��5�      P� 5�       � {� .  . �5�   "   ��  � ��
 �����6?   5�      5�      5�      5�      5�      5�      