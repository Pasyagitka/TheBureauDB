   M S B A M L   `   `   `  ����D  @TheBureau, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:TheBureau.ViewsTheBureau.Views  5   	   ;"clr-namespace:TheBureau.ViewModelsTheBureau.ViewModels  5   	   ;"clr-namespace:TheBureau.ConvertersTheBureau.Converters  5	   	   0    *TheBureau.Views.AdminWindow.MainWindowView   5   	   W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   /localclr-namespace:TheBureau.Views     5   	   _ [MaterialDesignThemes.Wpf, Version=4.2.1.2, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	   3
viewModels"clr-namespace:TheBureau.ViewModels  5   	   3
converters"clr-namespace:TheBureau.Converters  5	   	     � WindowStyle$
  None=�5
       � Title$ Статистика��5   	   $	��720��6   $
��1280��6,   
������  � 
ResizeMode$ 	CanResize=�5   	    � WindowStartupLocation$ CenterScreen=�5   	    � WindowState 5   "   �� *WindowState+��5   	   ��  �� Source# Z����5   	   �� # Z�� � DataContext 5      .   (TheBureau.ViewModels.MainWindowViewModel  5   
    � 	Resources 5         OpenMenu �� TargetProperty	 �� 
TargetName9  3System.Windows.Media.Animation.EasingDoubleKeyFrame
 O� KeyTime O� Value  	CloseMenu��%�  &	        &	 �     �� 5   
   .��5      U� $ (FrameworkElement.Width)�6,   $	 GridMenu��6a   .\�5        $
 0��6'   $ 70Q�63     5      $
 0:0:0.2��6'   $	 200Q�69   5      5      �� 5   
   .��5      U� $ (FrameworkElement.Width)�6,   $	 GridMenu��6a   .\�5        $
 0��6'   $	 200Q�63     5      $
 0:0:0.2��6'   $ 70Q�69   5      5       5!       � Triggers 5"      :� 5#   
    :� RoutedEvent$ ButtonBase.Click��6    :� 
SourceName$ ButtonOpenMenu��66   .[�5$      �� #��[  6   5%      :� 5&   
   $ ButtonBase.Click��6   $ ButtonCloseMenu��66   .[�5'      �� #��[ 6   5(      .��5*      � -   5        $��MainWindowGrid��5*      
������� � RowDefinitions 5+   
   �� 5,      $��30 �6   �� 5-      $��* �6   .+�50   
    � $
��1210��6   $	��690��6    $��	70, 0,0,0q�60   $��1��6C     � NavigationUIVisibility$ Hidden=�6y   ��6P   �� *	Content+ �� Mode$ TwoWay=���51      �� 52       �� ImageSource$+ %../../Resources/Images/background.png��6    �� Opacity$	 0.1Q�6Q   6a   54      � 57   
   -   5        $��TopGrid��57   -   $��0��6   $��0��6    $��30��6<   $	��Top=�6H     DarkMainGrayColor#���  6`   .+�58      �� $��Right=�6    �� Orientation$ 
Horizontal=�65   �� 59      $ 
Horizontal=�6   $��Right=�62   .+�5:      �� $��50��6     DefaultButtonGray#��[ 6%   
������ ��6i   �� *LogOutCommand+.��5;      '  !MaterialDesignThemes.Wpf.PackIcon     Kind$ 	ExitToApp=�6.   
�������6?   5<      �� 5=      $��50��6   $
��auto��6$   
������ 
������ $��Right=�6f   ��5>      �� *MinimizeWindowCommand+.��5?        $ WindowMinimize=�6.   
�������6E   5@      �� 5A      -   5        $��MainWindowResize��5A      $��50��61   $
��auto��6=   
������ 
������ $��Right=�6~   ��5B      �� *MaximizeWindowCommand+.��5C        -   5        $��MainWindowResizeIcon��5C   .   $ Resize=�6K   
�������6Y   5D      �� 5F      -   5        $��MainWindowClose��5G      $��50��6-   $
��auto��69   
������ 
������ $��Right=�5H   .   ��5F      �� *CloseWindowCommand+.��5I        $ Close=�6.   
�������6;   5J      5K      �� 5L      -   5        $��MainTopName��5L      $��Center=�6+   $��Center=�6H   $��20(�6c   
���������6�   �� *MainTopText+6�   5M      � 5N   
   -   5         �Name$ GridMenu��5N   =   $��0��6   $��2��6   $��0��6-   $��70��6O   $
��Left=�6Z   #���  6u    K� ZIndex$ 10��6�   .+�5O      �� .+�5P      � $	��150��6   
������� J� RenderTransformOrigin$ 0.5,0.5&�67   z�5Q      V� 5R      .~�5S      �� �� 5T      �� 5U      T� 5V      5W      .+�5Y      �� -   5         ��Name$ ButtonOpenMenu��5Y       � ToolTip$ Развернуть��65   $��60��6J   $��60��6V   $��5q�6a   $��Right=�6l   $	��Top=�6�     MaterialDesignFlatButton#��[ 6�   .��5Z      �� $%��../../Resources/Images/list.png��6    5[      �� 5\      -	   5        $ ButtonCloseMenu��5\      $y�	Collapsed=�66   $��60��6M   $��60��6Y   $��5q�6d   $��Right=�6o   $	��Top=�6�   
������ $��0q�6�   .��5]      �� $%��../../Resources/Images/back.png��6    $��30��6I   5^      �� 5_      $��80��6   $��Bottom=�6(   5`      �� 5a      -
   5        $��SideMenuListView��5a       � Cursor$
 Hand��6i   
�������$��Disabled=�6�    �� SelectedIndex 63   �� *SelectedIndex+$ TwoWay=�.@�5b      �� $��60��6#    �� 
IsSelected$
 True��6/   $ Статистика��6A   .��5c      �� $ 
Horizontal=�6%   .+�5d      �� $%��../../Resources/Images/stat.png��6$   �� 5e      $��Главная��6(   $��Center=�67   $��20 10q�6R   5f      5g      �� 5h      $��60��6#   $ Заявки��6/   .��5i      �� $ 
Horizontal=�6%   .+�5j      �� $'��!../../Resources/Images/orders.png��6$   �� 5k      $��Заявки��6(   $��Center=�66   $��20 10q�6R   5l      5m      �� 5n      $��60��6#   $ Бригады��6/   .��5o      �� $ 
Horizontal=�6%   .+�5p      �� $(��"../../Resources/Images/workers.png��6$   �� 5q      $��Бригады��6(   $��Center=�67   $��20 10q�6S   5r      5s      �� 5t      $��60��6#   $ Клиенты��6/   .��5u      �� $ 
Horizontal=�6%   .+�5v      �� $(��"../../Resources/Images/clients.png��6$   �� 5w      $��Клиенты��6(   $��Center=�67   $��20 10q�6S   5x      5y      �� 5z      $��60��6#   $ Работники��6/   .��5{      �� $ 
Horizontal=�6%   .+�5|      �� $-��'../../Resources/Images/employeecard.png��6$   �� 5}      $��Работники��6(   $��Center=�69   $��20 10q�6U   5~      5      �� 5�      $��60��6#   $ 
Склад��6/   .��5�      �� $ 
Horizontal=�6%   .+�5�      �� $$��../../Resources/Images/box.png��6$   �� 5�      $��
Склад��6(   $��Center=�65   $��20 10q�6Q   5�      5�      5�      5�      5�      5�      5�      