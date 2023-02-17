Attack lightning = new Attack("Lightning", 10);
Attack fire = new Attack("Fire", 25);
Attack ice = new Attack("Ice", 50);
Attack dark = new Attack("Dark", 35);
Attack earth = new Attack("Earth", 30);
Attack wind = new Attack("Wind", 20);
Attack water = new Attack("Water", 30);
Attack light = new Attack("Light", 40);

Enemy imp = new Enemy("Flying Imp");

imp.Attacks.Add(lightning);
imp.Attacks.Add(fire);
imp.Attacks.Add(ice);
imp.Attacks.Add(dark);
imp.Attacks.Add(earth);
imp.Attacks.Add(wind);
imp.Attacks.Add(water);
imp.Attacks.Add(light);

imp.RandomAttack();