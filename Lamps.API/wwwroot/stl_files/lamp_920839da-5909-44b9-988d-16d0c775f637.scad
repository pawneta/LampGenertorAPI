
module lamp(r_top, r_lamp, r_middle, r_base, h_top, h_lamp, h_base_top, h_base_bottom) {
    
    // Poprawiona formuła r_upper_lamp
    r_upper_lamp = r_top+(((r_lamp - r_top)*h_top) / (h_lamp + h_top));
    
    // Podstawa
    cylinder(h_base_bottom, r1=r_base, r2=r_middle);

    // Górna część podstawy
    translate([0,0,h_base_bottom])
    cylinder(h_base_top, r1=r_middle, r2=r_lamp);

    // Główna część lampy
    translate([0,0,h_base_bottom + h_base_top])
    cylinder(h_lamp, r1=r_lamp, r2=r_upper_lamp);

    // Górna część lampy
    translate([0,0,h_base_bottom + h_base_top + h_lamp])
    cylinder(h_top, r1=r_upper_lamp, r2=r_top);
}

lamp(
    r_top = 4,
    r_lamp = 5,
    r_middle = 6,
    r_base = 7,
    h_top = 8,
    h_lamp = 10,
    h_base_top = 4,
    h_base_bottom = 5
);