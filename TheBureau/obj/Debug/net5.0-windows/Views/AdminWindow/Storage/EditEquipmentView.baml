   M S B A M L   `   `   `  ����  	TheBureau3    -TheBureau.Views.AdminWindow.EditEquipmentView   5   	   W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   $	��390��5   	   $	��250��6     � WindowStyle$
  None=�6"    � 
ResizeMode$ NoResize=�5   	   
������  � AllowsTransparency$
 True��68    � Title$9 3Редактировать оборудование��5	   	    � WindowStartupLocation$ CenterScreen=�6,   .��5
      ��    LightGreenColor#���   6    �� CornerRadius$ 10��6<   ._�5   
   �� $ 10��6   
�������$��10q�66   ._�5      �� .+�5      �� $��40��6   $��16(�6(   $��Center=�6J   ��66   �� *Id+6J   �� 5      $��Тип прибора��6   $��Center=�6/   �� 5      $	��300��6   $��40��6&    $  TextBoxValidationTemplateDefault#��[ 6m   ��62   �� *Type+ �� UpdateSourceTrigger$ PropertyChanged=�6m   �� 5      $!��Способ монтажа��6   $��Center=�62   �� 5      $	��300��6   $��30��6'    �� SelectedValuePath$ id��6�   $ �� IsSynchronizedWithCurrentItem$
 TrueY�6�   $��5q�5   $   	 �� DisplayMemberPath$	 id��6v   
 �� SelectedValue
 5   3   �� *SelectedMountingId+$ PropertyChanged=���5   /   �� *	Mountings+$ PropertyChanged=�6v   �� 5       �� Orientation$ 
Horizontal=�6   .+�5      �� $��40��6   $	��160��6)   $��Отменить��65    �� IsCancel$
 True��6H     DefaultButtonGray#��[ 6X   $��10q�6�   6�   �� 5      -   5        $
��Save��5      $��Сохранить��6)   $	��160��6>   #��[ 6�   $��10q�6�   ��6J   �� *EditEquipmentCommand+6�   5      5      5      5      5      