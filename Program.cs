using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace project
{
  public class Rand
  {
    public int Run(int min, int max)
    {
      int range = (max - min) + 1;
      Random rng = new Random();
      return min + rng.Next() % range;
    }
  }

  public class Hero
  {
    public string Name;
    private int Strength;
    private int Dexterity;
    private int Intelligence;
    public double HP;
    public double MP;
    public static Hero Load(string name)
      {
        name = name + ".json";
        string heroString = File.ReadAllText(name);
        JObject heroJson = JObject.Parse(heroString);
        Hero hero = new Hero();
        hero.Name = (string)heroJson["Name"];
        hero.Strength = (int)heroJson["Strength"];
        hero.Dexterity = (int)heroJson["Dexterity"];
        hero.Intelligence = (int)heroJson["Intelligence"];
        return hero;
      }

    private void Init(int strength = 10, int dexterity = 10, int intelligence = 10)
    {
      this.Strength = strength;
      this.Dexterity = dexterity;
      this.Intelligence = intelligence;
      HP = 50 + strength;
      MP = 10 + (3 * intelligence);
    }

    public int GetStrength() { return this.Strength; }
    public int GetDexterity() { return this.Dexterity; }
    public int GetIntelligence() { return this.Intelligence; }

    public void UpStrength() { this.Strength += 5; this.HP += 5; }
    public void UpDexterity() { this.Dexterity += 5; }
    public void UpIntelligence() { this.Intelligence += 5; this.MP += (3 * this.Intelligence); }

    public Hero(string name = "", string myclass = "")
    {
      
      Name = name;
      switch(myclass)
      {
        case "warior": Init(15, 10, 5); break;
        case "assassin": Init(5, 15, 10); break;
        case "sorcerer": Init(5, 5, 20); break;
        default: Init(); break;
      }
    }
        public void Attack(Hero enemy)
    {
      Rand rand = new Rand();
      double damage = Strength * rand.Run(5, 10) / 10;

      if(rand.Run(0, 100) > enemy.GetDexterity())
      {
        Console.WriteLine("Bang!");
        enemy.HP -= damage;
      }
      else Console.WriteLine("Dodge!");
    }

    public void LevelUp()
    {
      Console.Write("  1:Strength, 2:Dexterity, 3:Intelligence ... ");
      int opt = int.Parse(Console.ReadLine());

      switch(opt)
      {
        case 1: UpStrength(); break;
        case 2: UpDexterity(); break;
        case 3: UpIntelligence(); break;
      }

      Console.WriteLine();
    }

        public void Spell(Hero enemy)
        {
            Console.WriteLine("1: Fireball, 2: Thunder, 3: Flames of hell, 4:Healing potion");
            int spell = int.Parse(Console.ReadLine());
            double damage = 0;
            double heal = 0;
            switch (spell)
            {
                case 1:
                    if(this.MP >= 9)
                    damage = this.Strength * 0.9;
                    enemy.HP -= damage;
                    if(this.MP >= 9)
                    this.MP -= 9;
                    Console.WriteLine("Fireball hits enemy for {0} damage", damage);
                    break;
                case 2:
                    if(this.MP >= 5)
                    damage = this.Strength * 0.5;
                    enemy.HP -= damage;
                    if(this.MP >= 5)
                    this.MP -= 5;
                    Console.WriteLine("Thunder hits enemy for {0} damage", damage);
                    break;
                case 3:
                    if(this.MP >= 20)
                    damage = this.Strength * 2;
                    enemy.HP -= damage;
                    if(this.MP >= 20)
                    this.MP -= 20;
                    Console.WriteLine("Flames of hell hit enemy for {0} damage", damage);
                    break;
                case 4:
                    if(this.MP >= 12)
                    heal = 6;
                    this.HP += heal;
                    if(this.MP >= 12)
                    this.MP -= 12;
                    Console.WriteLine("You healed for {0} HP", heal);
                    break;
                default:
                    Console.WriteLine("Wrong spell selection");
                    break;
            }
    }

        public void PerRound()
        {
          if (new Rand().Run(1, 101) <= 50) 
          {
              this.HP += (this.Intelligence / 10);
              this.MP += (this.Intelligence / 20);
          }
            // 1 in 100 chance -HP
            if (new Random().Next(1, 101) == 100) 
            {
              this.HP -= 1;
            }
        }
    }

  class Program
  {
    static void Main(string[] args)
    {
      Hero hero1 = Hero.Load("hero");
      int tour = 1;

      //Hero hero = new Hero("Edward Białykij", "sorcerer");
      Console.WriteLine(hero1.Name + " Str:{0} Dex:{1} Int:{2} HP:{3} MP:{4}", hero1.GetStrength(), hero1.GetDexterity(), hero1.GetIntelligence(), hero1.HP, hero1.MP);

      Hero hero2 = new Hero("Wataszka Stefan", "assassin");
      Console.WriteLine(hero2.Name + " Str:{0} Dex:{1} Int:{2} HP:{3} MP:{4}", hero2.GetStrength(), hero2.GetDexterity(), hero2.GetIntelligence(), hero2.HP, hero2.MP);

      Console.WriteLine();

      while(hero1.HP > 0 && hero2.HP > 0)
      {
        hero1.PerRound();
        hero2.PerRound();
        if(tour == 1) Console.WriteLine("Your Turn: " + hero1.Name);
        else Console.WriteLine("Your Turn: " + hero2.Name);

        Console.Write("1:Attack, 2:Spell, 3:LevelUp ... ");
        int opt = int.Parse(Console.ReadLine());

        switch(opt)
        {
          case 1:
            if(tour == 1) hero1.Attack(hero2);
            else hero2.Attack(hero1);
          break;

          case 2:
            if(tour == 1) hero1.Spell(hero2);
            else hero2.Spell(hero1);
          break;

          case 3:
            if(tour == 1) hero1.LevelUp();
            else hero2.LevelUp();
          break;
        }

        Console.WriteLine(hero1.Name + " Str:{0} Dex:{1} Int:{2} HP:{3}", hero1.GetStrength(), hero1.GetDexterity(), hero1.GetIntelligence(), hero1.HP);
        Console.WriteLine(hero2.Name + " Str:{0} Dex:{1} Int:{2} HP:{3}", hero2.GetStrength(), hero2.GetDexterity(), hero2.GetIntelligence(), hero2.HP);
        Console.WriteLine();


        tour++;
        if(tour > 2) tour = 1;
      }
      while(hero1.HP > 0 && hero2.HP > 0)
{
                Console.WriteLine("Tour {0}", tour);
                Console.WriteLine("{0} HP:{1} MP:{2}", hero1.Name, hero1.HP, hero1.MP);
                Console.WriteLine("{0} HP:{1} MP:{2}", hero2.Name, hero2.HP, hero2.MP);
                Console.WriteLine();

                Console.WriteLine("{0}'s turn:", hero1.Name);
                Console.WriteLine(" 1:Attack, 2:Level Up, 3:Spell");
                int opt = int.Parse(Console.ReadLine());

                if (opt == 1) hero1.Attack(hero2);
                else if (opt == 2) hero1.LevelUp();
                else if (opt == 3) hero1.Spell(hero2);
                hero1.PerRound();

                if (hero2.HP > 0)
                {
                    Console.WriteLine("{0}'s turn:", hero2.Name);
                    Console.WriteLine(" 1:Attack, 2:Level Up, 3:Spell");
                    opt = int.Parse(Console.ReadLine());
                    if (opt == 1) hero2.Attack(hero1);
                    else if (opt == 2) hero2.LevelUp();
                    else if (opt == 3) hero2.Spell(hero1);
                    hero2.PerRound();
                }

                tour++;
            }

            if (hero1.HP > 0) Console.WriteLine("{0} wins!", hero1.Name);
            else Console.WriteLine("{0} wins!", hero2.Name);
    }
  }
}




