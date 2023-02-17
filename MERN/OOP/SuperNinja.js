class Ninja {
    constructor(name, health) {
        this.name = name;
        this.health = health;
        this.speed = 3;
        this.strength = 3;
    }

    sayName() {
        console.log(this.name);
    }
    showStats() {
        console.log(`Name: ${this.name}, Str: ${this.strength}, Speed: ${this.speed}, Health: ${this.health}`)
    }
    drinkSake() {
        this.health += 10;
    }
}

class Sensei extends Ninja {
    constructor(name) {
        super(name, 200);
        this.speed = 10;
        this.strength = 10;
        this.wisdom = 10;
    }

    speakWisdom() {
        super.drinkSake();
        console.log('Sake opens the mind of those who drink!')
    }
}

const sensei = new Sensei("Mr. Miyagi");
sensei.speakWisdom();
sensei.showStats();