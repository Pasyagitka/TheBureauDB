   M S B A M L   `   `   `  ����D  @TheBureau, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:TheBureau.ControlsTheBureau.Controls  5   	   ;"clr-namespace:TheBureau.ViewModelsTheBureau.ViewModels  5   	   ] YMicrosoft.VisualBasic, Version=10.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a\Bclr-namespace:Microsoft.VisualBasic;assembly=Microsoft.VisualBasicMicrosoft.VisualBasic 6@   '    !TheBureau.Views.BrigadeWindowView   5   	   6c1/http://schemas.componentone.com/winfx/2006/xaml  W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   5controls clr-namespace:TheBureau.Controls     5   	   _ [MaterialDesignThemes.Wpf, Version=4.2.1.2, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  6;   3
viewModels"clr-namespace:TheBureau.ViewModels  5   	   JiBclr-namespace:Microsoft.VisualBasic;assembly=Microsoft.VisualBasic  6@     � Title$  Бригада��5	   	   $	��715��6   $
��1000��6&   $��15(�63   
������  � AllowsTransparency$
 True��6Z    � WindowStartupLocation$ CenterScreen=�6t    � WindowStyle$
 None=�5
   	    � 
ResizeMode$ NoResize=�6    � WindowState 5      �� *WindowState+.��5      ��    NiceDarkWhiteColor#���   6    �� CornerRadius$ 15��6?   ._�5   
   � .+�5      � -   5        $��TopGrid��5      $	��Top=�6"   .+�5      ]� -   5        $��MainWindowTop��5   2   $��False��6   $��25��6G   
������ $��15,0,12,-0.2q�6m   .+�5      �� $��70��6     NiceGrayColor#���  6(     DefaultButtonGray#��[ 6U   
������ ��6�   �� *LogOutCommand+.��5      �� $%��../../Resources/Images/exit.png��6    $��20��6I   5      �� 5      -   5        $��MainWindowClose��5      $��30��65   $
��auto��6A   
������ 
������ $��Right=�6�   $��
0,0,0,-0.2q�6�   $��0q�6�    � Cursor$
 Hand��6�   ��6�   �� *CloseWindowCommand+.��5      '  !MaterialDesignThemes.Wpf.PackIcon     Kind$ Close=�62   $��0q�6?   $��24��6J   #���  6U   $��17��6�   5      �� 5      -   5        $��MainWindowMinimize��5      $��30��68   $
��auto��6C   
������ 
������ $��Right=�6�   $��0q�6�   $
 Hand��6�   ��6�   �� *MinimizeWindowCommand+.��5        $��24��62   $ WindowMinimize=�6=   #���  6T   $��16��6�   5      5      5      ]� 5      -   5        $��MainWorkspace��5      $	��Top=�6-   $
��True��6B   .+�5      �� $��50��6   $��
50,40,50,0q�6%   $ 5��69   $	��Top=�6J   ��5       �� 5!      .��5"      �   LightOrange	 � Color#	 �  6+   
 � Offset$	
 0.0Q�6Q   � 5#        
LightGreen#	 �  6+   $
 1Q�6P   5$      ._�5&      ]� $
��True��6    $	��Top=�65   .+�51      �� $��Right=�6!   $ 2��68   ._�52      �� $+��%../../Resources/Images/logo_light.jpg��6$   $��3q�6T   53      �� 54      $��БРИГАДА��6$   $��Center=�63   
�������$
��Bold"�6a   $��Center=�6s   55      56      �� 58      $	��Top=�6   .+�59      �� $��Stretch=�6   $	��130��6<   $	��900��6I   $��10q�6U   
�������$ 15��6u   ._�5:      �� .��5;      �� -   5         ��Name$ A��5;   &    �� SelectionMode$ Multiple=�62    �� DisplayMemberPath$ surname��6|   ��6K   �� *CurrentBrigade.Employees+6�   5<      5=      5>      �� 5H      $	��Top=�6   5N      �� 5O      $	��Top=�6    �� Orientation$ 
Horizontal=�62   $��Center=�6K   .+�5P      ��  � ToolTip$0 *Изменить статус заявки��6   $��0q�6>   $��5q�6J   $��Center=�6U   $��50��6r   $��50��6~     MaterialDesignIconButton#��[ 5Q   F   ��6   �� *UpdateRequestCommand+.��5R      �� $%��../../Resources/Images/edit.png��6    $��5q�6J   5S      d� 5T      -   5        $��
InfoToggle��5T   E   $3 -Развернуть оборудование��6#   $
 Hand��6W   .��5U        $ InfoVariant=�63   $��21��6F   $��21��6R   5V      �� 5W      #��[ 6E   $
 Hand��6w   ��6   �� *OpenScheduleCommand+.��5X      �� $,��&../../Resources/Images/allrequests.png��6    $��5q�6Q   5Y      �� 5Z      -   5        $
 Hand��5Z   2   #��[ 6@   .��5[      �� $(��"../../Resources/Images/workers.png��6    $��5q�6M   5\      5]      �� 5^      $	��Top=�6   $��10q�6�   $	��800��6�     PrimaryLightGrayColor#���  6�   y�63   ��  �� ElementName$ 
InfoToggle�� �� Path$ 	IsChecked�   BooleanToVisibilityConverter �� 	Converter# [ .+�5_      &   System.Windows.Controls.DataGrid     AutoGenerateColumns$ False��6I      
IsReadOnly$
 True��6e   $��Center=�6w      CanUserReorderColumns$
 True��6�      ColumnWidth5  /System.Windows.Controls.DataGridLengthConverter$ * 6�   ��6   �� *RequestEquipments+   Columns 5`      0  *System.Windows.Controls.DataGridTextColumn  5a      ,  &System.Windows.Controls.DataGridColumn   Header$ Оборудование��61   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6G   �� *Equipment.type+  5b      $! Способ монтажа��61    6I   �� *Equipment.mountingName+  5c      $ Количество��61    6F   �� *
quantity+5e      5f      �� 5g      $	��Top=�6   
�������$ 15��6A   $��50,10,50,20q�6S   ._�5h      �� 1  +MaterialDesignThemes.Wpf.ScrollViewerAssist   IsAutoHideEnabled$
 True��6#   .��5i      �� $
��True��6#   $��Center=�6H    �� SelectedItem 5j   #   �� *SelectedItem+ �� Mode$ TwoWay=���5k   #   �� *BrigadeRequests+��5l      �� 5m   "   .R�5n   &   �� $ 10��6-   
�������$��10q�6R   $	��800��6^   $��Center=�6j   ._�5o   *   �� $ Vertical=�65   $��5q�6L   $��Center=�6W   .+�5p   .   $   TheBureau.Controls.RequestCard  $��Center=�6C   �� 5q   .   $	��500��68     LightOrangeColor#��[ 6D   5r   +   5s   '   5t   #   5v      5w      5x      5y      5z      5{      5|      