   M S B A M L   `   `   `  ����D  @TheBureau, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:TheBureau.ViewsTheBureau.Views  5   	   >    8TheBureau.Views.BrigadeWindow.EditRequestFromBrigadeView   5   	   W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   /localclr-namespace:TheBureau.Views     5   	     � 
ResizeMode$  NoResize=�5       � Title$ EditRequestView��5   	   $	��315��6!   $��90��6-    � WindowStyle$
 None=�69    � WindowStartupLocation$ CenterScreen=�6L   .��5	      ��    LightGreenColor#���   6    �� CornerRadius$ 5��6=   ._�5
   
   �� $ 5��6   
�������$��7q�65   ._�5   
   ��  �� Orientation$ 
Horizontal=�6   .+�5      �  � ColumnDefinitions 5      �� 5      $	��200 �6'   �� 5      $
��Auto �6'   �� 5       � RowDefinitions 5      �� 5      $��20 �6$   �� 5      .+�5      �� $��0��6   $��0��6,   $.��(Сменить статус заявки��69   $��Center=�6V   $��3q�6s   6�   �� 5      $��0��6   $��1��6-   .+�5      �� $��5q�6   $��25��6*   $ �� IsSynchronizedWithCurrentItem$
 TrueY�6^   	 �� SelectedValue	 66   �� *RequestStatus+.@�5      �� $��В обработке��6'     LightOrangeColor#���  6=   �� 5      $��В процессе��6'     LightYellowColor#���  6<   �� 5      $��Готово��6'   #���   68   5      5      �� 5      $��2��6   $��0��6-   $��2��6:   $ 
Horizontal=�6K   .+�5      �� -   5        $��0q�5      
�������$��5, 0,0,0q�6D   $��Center=�6V     MaterialDesignIconButton#��[ 6s   ��5       �� *UpdateRequestCommand+.��5!      �� $&�� ../../Resources/Images/check.png��6    $��40��6J   5"      �� 5#      -   5        $��0q�5#      
�������$��
0,10,10,10q�6D   $��Center=�6X   #��[ 6u   .��5%      �� $'��!../../Resources/Images/delete.png��6    $��40��6K   5&      5'      �� 5(      $��1��6   $��1��6-   .+�5)      �� -   5        $ ��ListBoxEmployeesAttendance��5)   5   $��5q�6   $��25��6)   
 �� SelectionMode$
 Multiple=�5*      $
 TrueY�5+   G   ��6   �� *BrigadeEmployees+5,      5-      5.      5/      50      51      52      