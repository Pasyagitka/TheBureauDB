   M S B A M L   `   `   `  ����D  @TheBureau, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:TheBureau.ViewsTheBureau.Views  5   	   7 clr-namespace:TheBureau.ControlsTheBureau.Controls  5   	   ;"clr-namespace:TheBureau.ViewModelsTheBureau.ViewModels  5	   	   3    -TheBureau.Views.ClientWindow.ClientWindowView   5   	   W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   _ [MaterialDesignThemes.Wpf, Version=4.2.1.2, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	   )localclr-namespace:TheBureau.Views  5   	   /controls clr-namespace:TheBureau.Controls  5   	   3
viewModels"clr-namespace:TheBureau.ViewModels  5	   	     � Title$  Клиенты��5   	   $	��715��5   	   $
��1000��6   $��15(�6#   
������  � AllowsTransparency$
 True��6J    � WindowStartupLocation$ CenterScreen=�6d    � WindowStyle$
 None=�5   	    � 
ResizeMode$ NoResize=�6    � WindowState 5      �� *WindowState+ � DataContext 5      0   *TheBureau.ViewModels.ClientWindowViewModel  5   
    � 	Resources 5         TB �� 
TargetType	 �� Property
 �� Value �� Triggers �� Binding �� ElementName �� Path �� Value  expandAreaStyle N� EnterActions �� TargetProperty	 t� To g� Duration N� ExitActions��%�  &	        &	 �     �� 5   
   " ��6   �5      �� 5      	 � y�$
 Hidden=�6/   6@    5      �� 5       6   �� $ RequestsListView��$ HasItems�	 False6^   .a�5      �� 	 � y�$
 Visible=�63   5      5      �� 5   
   " ��6(   .�5      �� 	 � ��$
 
1000 0 0 0q�6'    5      �� 5        6   �� *	IsChecked+$ expandButton�� True6V    5!      �� 5"      .��5#      �� $ Margin�6)   .��5$   "   t�  	550 0 0 065   $ 0:0:0.2D�6D   5%      5&       5(      �� 5)      .��5*      �� $ Margin�6)   .��5+   "   t�  
1000 0 0 065   $ 0:0:0.2D�6E   5,      5-      5/      51      52      .��53      ��   NiceDarkWhiteColor#���  6    �� CornerRadius$ 15��6?   ._�54   
   � $��0,0,0,50q�6   .+�55      ]�  K� ZIndex$ 3��6   $��	0,0,0,-50q�6*   .+�56      �� -   5        $��	SidePanel��56   0   $��Right=�6   #��[ 6A   ._�57      $   TheBureau.Controls.RequestForm  58      59      � 5:      -   5        $��TopGrid��5:      $	��Top=�6#   .+�5;      ]� -   5        $��MainWindowTop��5;   2   $��False��6   $��25��6G   
������ $��	15,0,12,0q�6m   .+�5<      �� -   5        $��MainWindowClose��5<      $��30��65   $
��auto��6A   
������ 
������ $
��Left=�6�   $��
0,0,0,-0.2q�6�   $��0q�6�    � Cursor$
 Hand��6�   ��6�   �� *CloseWindowCommand+.��5=      '  !MaterialDesignThemes.Wpf.PackIcon     Kind$ Close=�62   $��0q�6?   $��24��6J     NiceGrayColor#���  6U   $��17��6�   5>      �� 5?      -   5        $��MainWindowMinimize��5?      $��30��68   $
��auto��6C   
������ 
������ $
��Left=�6�   $��0q�6�   $
 Hand��6�   ��6�   �� *MinimizeWindowCommand+.��5@        $��24��62   $ WindowMinimize=�6=   #���  6T   $��16��6�   5A      �� 5B      $��30��6   #���  6(     DefaultButtonGray#��[ 6U   
������ ��6�   �� *LogOutCommand+.��5C      �� $%��../../Resources/Images/exit.png��6    $��20��6I   5D      5E      5F      �� 5G      -   5        $��MainWorkspace��5G      $	��Top=�6.   .+�5H      �� $��50��6   $��
50,40,50,0q�6%   $ 5��69   ��5I      �� 5J      .��5K      �   LightOrange � Color# �  6+    � Offset$	 0.0Q�6Q   � 5L        
LightGreen# �  6+   $ 1Q�6P   5M      ._�5O      ]� $��False��6    .+�5P      �� 
�������$ 5��64   $��10, 10, 0, 10q�6E   ._�5Q      �� $��5q�6&   )  #MaterialDesignThemes.Wpf.HintAssist   Hint$7 1Найти заявку (номер заявки)��62   .  (MaterialDesignThemes.Wpf.TextFieldAssist   DecorationVisibility$ Hidden=�5R   )   $��0q�5S   )   $	��230��5T   )   $��Center=�5U   )   $ IBeam��5V   o   ��6)   �� *FindRequestText+ �� UpdateSourceTrigger$ PropertyChanged=�6o   5W        5X      $ Search=�62   
�������$��29��6T   $
��Auto��6_   $��0,11,0,10.6q�6m   d� 5Y      -   5         d�Name$ expandButton��5Y   '   $%��Оставить заявку >��6=   $	��237��6\   $��50��6h   $
��Left=�6t    ) %MaterialDesignActionLightToggleButton#��[ 6�   1  +MaterialDesignThemes.Wpf.ToggleButtonAssist   	OnContent$* $Скрыть форму заявки��5Z   (   
������ $��
0,0,0,-0.4q�6�   �� 5[      $��Right=�6!   $ 2��68   ._�5\      �� $+��%../../Resources/Images/logo_light.jpg��6$   $��3q�6T   5]      5^      5_      �� 5`      $��50,30,50,50q�6   
�������$ 15��6B   $	��530��6T   ._�5a      � .+�5b      �� #��[  6$     �� TextAlignment$  Center=�6@   $��Center=�6W   $��60��6t   $��20(�6�   $��WrapWithOverflow=�6�   $	��500��6�   $|��vНет заявок для отображения. Введите номер заявки в строку поиска��6�   �� 5c      1  +MaterialDesignThemes.Wpf.ScrollViewerAssist!   IsAutoHideEnabled$
! True��6'   .��5d      �� -   5        $��RequestsListView��5d   '   $��Center=�6m   ��6?   �� *
Requests+" �� Mode$" TwoWay=���5e   "   �� 5f   &   .R�5g   )   $   TheBureau.Controls.RequestCard  5h   '   5j      5k      5l      5m      5n      5o      5p      5q      