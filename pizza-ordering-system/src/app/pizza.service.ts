import { Injectable } from '@angular/core';
import { Pizza, PizzaSize, OFFERS, TOPPINGS, SIZES } from './models/pizza';

@Injectable({
  providedIn: 'root'
})
export class PizzaService {

  calculatePrice(pizza: Pizza): number {
    const sizePrice = SIZES[pizza.size as PizzaSize];
    let price = sizePrice;
  
    for (const topping of pizza.toppings) {
      const toppingInfo = TOPPINGS.find(t => t.name === topping);
      if (toppingInfo) {
        price += toppingInfo.price;
      }
    }
  
    return price;
  }

  checkOffers(pizza: Pizza, orderCount: number = 1): { appliedOffer?: string, originalPrice: number, finalPrice: number } {
    const sizePrice = SIZES[pizza.size as keyof typeof SIZES];
    const pizzaPrice = this.calculatePrice(pizza);
    let appliedOffer: string | undefined;
    let finalPrice = pizzaPrice;
    
    // Offer 1: 1 Medium Pizza with 2 toppings = $5
    if (pizza.size === 'medium' && pizza.toppings.length === 2) {
      appliedOffer = 'OFFER1';
      finalPrice = OFFERS.OFFER1.price;
    }

    // Offer 2: 2 Medium Pizzas with 4 toppings each = $9
    else if (orderCount === 2 && pizza.size === 'medium' && pizza.toppings.length === 4) {
      appliedOffer = 'OFFER2';
      finalPrice = OFFERS.OFFER2.price;
    }

    // Offer 3: 1 Large Pizza with 4 toppings (Pepperoni and Barbecue Chicken count as 2 toppings) - 50% discount
    else if (pizza.size === 'large' && pizza.toppings.length === 4) {
      const specialToppingCount = pizza.toppings.filter(topping => OFFERS.OFFER3.specialToppings.includes(topping)).length;
      const adjustedToppingCount = specialToppingCount >= 2 ? 2 : pizza.toppings.length;
      const offerPrice = sizePrice + (pizzaPrice - sizePrice);
      finalPrice = offerPrice * OFFERS.OFFER3.discount;
      appliedOffer = 'OFFER3';
    }

    return {
      appliedOffer,
      originalPrice: pizzaPrice,
      finalPrice
    };
  }
}
