   M S B A M L   `   `   `  ����D  @TheBureau, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:TheBureau.ViewsTheBureau.Views  5      ;"clr-namespace:TheBureau.ViewModelsTheBureau.ViewModels  5      -    'TheBureau.Views.AdminWindow.StorageView   5      W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      /localclr-namespace:TheBureau.Views     5      _ [MaterialDesignThemes.Wpf, Version=4.2.1.2, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5      3
viewModels"clr-namespace:TheBureau.ViewModels  5        � DataContext  5      +   %TheBureau.ViewModels.StorageViewModel  5   
   .��5      ��  �� CornerRadius$ 15��6   $��10q�6   ._�5   
   ]� -   5        $��TablesDockPanel��5   )   $��10,10,10,20q�6   .+�5      �� $��0q�6   $	��Top=�6-    �� TabStripPlacement$	 Top=�6B   .  (MaterialDesignThemes.Wpf.ColorZoneAssist   Mode$ Standard=�6Z   ��6�   ��  �� ElementName$ TablesDockPanel�� �� Path$ ActualWidth�.@�5      �� $	��220��6   $��Инструменты��6&    '  #MaterialDesignNavigationRailTabItem#��[  6;    � Cursor$
 Hand��6x   .��5      �� 
�������$ 10��60   $	��900��6B   ._�5      ]� .+�5      `� $	��Top=�6*   .�5   "   c�   MaterialDesignToolBar#���  6*   $}�False��6Z   .@�5   &   ��  � ToolTip$ Добавить��6-   $
 Hand��6m   ��6@   �� *OpenAddToolWindowCommand+.��5   *   �� $'��!../../../Resources/Images/add.png��60   $��30��6[   5   '   �� 5   &   $  Редактировать��6-   $
 Hand��6s   ��6E   �� *OpenEditToolWindowCommand+.��5   *   �� $(��"../../../Resources/Images/edit.png��60   $��30��6\   5   '   �� 5   &   $ Удалить��6-   $
 Hand��6e   ��6?   �� *DeleteToolCommand+.��5   *   �� $*��$../../../Resources/Images/delete.png��60   $��30��6^   5   '   �� 5   &   $ Импорт��6.   $
 Hand��6e   ��6?   �� *ImportCommandTool+.��5    *   �� $*��$../../../Resources/Images/import.png��60   $��30��6^   5!   '   �� 5"   &   $ Экспорт��6-   $
 Hand��6e   ��6?   �� *ExportCommandTool+.��5#   *   �� $*��$../../../Resources/Images/export.png��60   $��30��6^   5$   '   5%   #   5&      �� 5.      $��0q�6o   $��Stretch=�6�   ��6'   �� *Tools+ �� SelectedItem 6E   �� *SelectedToolItem+	 �� View	 5/   "   �� 50   &   .P�51   *   �� $��ID��6:   
 �� DisplayMemberBinding
 6F   �� $ id��� 52   *   $��Наименование��6:    �� Width$	 200��6{   
 6P   �� $
 name�6{   �� 53   *   $T��NСтадия отделки (1-черновая, 2-чистовая, 3-обе)��6:   
 6r   �� $ stage�54   '   56      57      58      59      �� 5:      $	��220��6   $ ��Комплектующие��6'   #��[  6>   $
 Hand��6{   .��5;      �� 
�������$ 10��61   $	��900��6C   ._�5<      ]� .+�5=      `� $	��Top=�6*   .�5>   "   c� #���  6*   $}�False��6Z   .@�5?   &   �� $ Добавить��6-   $
 Hand��6r   ��6@   �� *OpenAddAccessoryWindowCommand+.��5@   *   �� $'��!../../../Resources/Images/add.png��60   $��30��6[   5A   '   �� 5B   &   $  Редактировать��6-   $
 Hand��6x   ��6E   �� * OpenEditAccessoryWindowCommand+.��5C   *   �� $(��"../../../Resources/Images/edit.png��60   $��30��6\   5D   '   �� 5E   &   $ Удалить��6-   $
 Hand��6j   ��6?   �� *DeleteAccessoryCommand+.��5F   *   �� $*��$../../../Resources/Images/delete.png��60   $��30��6^   5G   '   �� 5H   &   $ Импорт��6.   $
 Hand��6j   ��6?   �� *ImportCommandAccessory+.��5I   *   �� $*��$../../../Resources/Images/import.png��60   $��30��6^   5J   '   �� 5K   &   $ Экспорт��6-   $
 Hand��6j   ��6?   �� *ExportCommandAccessory+.��5L   *   �� $*��$../../../Resources/Images/export.png��60   $��30��6^   5M   '   5N   #   5O      �� 5P      $��0q�6|   ��6(   �� *Accessories+ 6M   �� *SelectedAccessoryItem+	 5Q      �� 5R   "   .P�5S   &   �� $��ID��65   $ 80��6A   
 6L   �� *id+�� 5T   &   $��Артикул��65   $	 120��6F   
 6R   �� *art+�� 5U   &   $��Наименование��65    �� CellTemplate 5V   *   �� 5W   .   .R�5X   2   �� $
��Wrap=�6<   ��6P   �� *name+5Y   /   5[   '   �� 5\   &   $��Цена��65   $	 100��6D   
 6P   �� *price+�� 5]   &   $��Тип прибора��65   $	 180��6J   
 6V   �� *equipmentId+5^   #   5`      5a      5q      5r      �� 5s      $	��220��6   $��Приборы��6'   #��[  68   $
 Hand��6u   .��5t      �� 
�������$ 10��67   $	��900��6I   ._�5u      ]� .+�5v      `� $	��Top=�6*   .�5w   "   c� #���  6*   $}�False��6Z   .@�5x   &   �� $ Добавить��6-   $
 Hand��6r   ��6@   �� *OpenAddEquipmentWindowCommand+.��5y   *   �� $'��!../../../Resources/Images/add.png��60   $��30��6[   5z   '   �� 5{   &   $  Редактировать��6-   $
 Hand��6x   ��6E   �� * OpenEditEquipmentWindowCommand+.��5|   *   �� $(��"../../../Resources/Images/edit.png��60   $��30��6\   5}   '   �� 5~   &   $ Удалить��6-   $
 Hand��6j   ��6?   �� *DeleteEquipmentCommand+.��5   *   �� $*��$../../../Resources/Images/delete.png��60   $��30��6^   5�   '   5�   #   5�      �� 5�      $��0q�6{   $��Center=�6�   ��6(   �� *
Equipments+ 6L   �� *SelectedEquipmentItem+	 5�      �� 5�   "   .P�5�   &   �� $��ID��65   $	 100��6A   
 6M   �� *id+�� 5�   &   $��Тип прибора��65   $	 300��6K   
 6X   �� *type+�� 5�   &   $!��Способ монтажа��65   $	 300��6M   
 6Z   �� *
mounting+5�   #   5�      5�      5�      5�      5�      5�      5�      5�      