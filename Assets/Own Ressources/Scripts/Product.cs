using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Product {
    public Ressources product;
    public int number = 1;  //Number per round
    public List<Cost> costs;

    public Product(Ressources product, Cost[] costs)
    {
        this.product = product;
        this.costs = new List<Cost>(costs);
    }

    public Product(Ressources product, int number, Cost[] costs) : this(product, costs)
    {
        this.number = number;
    }




    /*
     * Products
     */
    public static Product cement = new Product(Ressources.Cement, 1, new Cost[] { new Cost(Ressources.Money, 3),
                                                                                  new Cost(Ressources.LimeStone, 1),
                                                                                  new Cost(Ressources.Clay, 2)});
    public static Product clay = new Product(Ressources.Clay, 1, new Cost[] { new Cost(Ressources.Money, 5)});
    public static Product concrete = new Product(Ressources.Concrete, 2, new Cost[] { new Cost(Ressources.Money, 4),
                                                                                      new Cost(Ressources.Cement, 1),
                                                                                      new Cost(Ressources.Gravel, 2),
                                                                                      new Cost(Ressources.Sand, 1),
                                                                                      new Cost(Ressources.Water, 2)});
    public static Product gravel = new Product(Ressources.Gravel, 1, new Cost[] { new Cost(Ressources.Money, 15) });
    public static Product limeStone = new Product(Ressources.LimeStone, 2, new Cost[] { new Cost(Ressources.Money, 10) });
    public static Product sand = new Product(Ressources.Sand, 2, new Cost[] { new Cost(Ressources.Money, 15) });
    public static Product water = new Product(Ressources.Water, 2, new Cost[] { new Cost(Ressources.Money, 2) });
    public static Product wood = new Product(Ressources.Wood, 1, new Cost[] { new Cost(Ressources.Money, 1) });
}
