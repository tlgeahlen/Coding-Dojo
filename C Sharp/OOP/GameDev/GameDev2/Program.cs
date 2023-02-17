
Melee tank = new Melee("Paladin");

tank.Attack();
tank.Rage();

Ranged corsair = new Ranged("Ranger");

corsair.Attack();
corsair.Dash();
corsair.Attack();

MagicCaster redmage = new MagicCaster("Red Mage");

redmage.Attack();
redmage.Heal(corsair);
redmage.Heal(redmage);