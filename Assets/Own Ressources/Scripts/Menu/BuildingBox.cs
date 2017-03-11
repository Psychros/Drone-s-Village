using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingBox : MonoBehaviour {

	public void changeProduct(string p)
    {
        print("HJDHJDD");
        Factory fac = (Factory)InputManager.instance.selectedBuilding;
        Product product = null;

        //Choose the correct data
        switch (p)
        {
            case "Cement": product = Product.cement; break;
            case "Clay": product = Product.clay; break;
            case "Concrete": product = Product.concrete; break;
            case "Gravel": product = Product.gravel; break;
            case "LimeStone": product = Product.limeStone; break;
            case "Sand": product = Product.sand; break;
            case "Water": product = Product.water; break;
            case "Wood": product = Product.wood; break;
        }

        //Use the coosen data that the factory can produce the choosen product
        fac.product = new Cost(product.product, product.number);
        fac.costs = product.costs;

        close();
    }

    public void close()
    {
        InputManager.instance.selectedBuilding = null;
        InputManager.instance.recalculateBuildingBox();
    }
}
