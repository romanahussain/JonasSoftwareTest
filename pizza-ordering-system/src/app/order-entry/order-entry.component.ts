import { Component } from '@angular/core';
import { PizzaService } from '../pizza.service';
import { PizzaSize, Topping, TOPPINGS, Pizza, SIZES } from '../models/pizza';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-order-entry',
  templateUrl: './order-entry.component.html',
  styleUrls: ['./order-entry.component.css'],
  standalone: true,
  imports: [CommonModule, FormsModule]
})
export class OrderEntryComponent {
  pizzaSizes: PizzaSize[] = ['small', 'medium', 'large', 'extraLarge'];
  selectedSize: PizzaSize = 'medium';
  originalPrice: number = 0;
  finalPrice: number = 0;
  appliedOffer: string = '';

  toppings: Topping[] = TOPPINGS;
  SIZES = SIZES;

  selectionState: { [toppingName: string]: { [size in PizzaSize]: boolean } } = {} as any;
  totalPrice: { [size in PizzaSize]: number } = {} as any;
  appliedOffers: { [size in PizzaSize]: string } = {} as any;
  originalPrices: { [size in PizzaSize]: number } = {} as any;
  finalPrices: { [size in PizzaSize]: number } = {} as any;

  constructor(private pizzaService: PizzaService) {
    // Initialize selectionState
    this.pizzaSizes.forEach(size => {
      this.totalPrice[size] = 0;      
      this.originalPrices[size] = 0;  
      this.finalPrices[size] = 0;     
      this.appliedOffers[size] = ''; 

      TOPPINGS.forEach(topping => {
        if (!this.selectionState[topping.name]) {
          this.selectionState[topping.name] = {} as any;
        }
        this.selectionState[topping.name][size] = false;
      });
    });
  }

  getDisplaySize(size: PizzaSize): string {
    const sizeMap: { [key in PizzaSize]: string } = {
      small: 'Small',
      medium: 'Medium',
      large: 'Large',
      extraLarge: 'Extra Large'
    };
    return sizeMap[size];
  }
  calculatePrice() {
    this.pizzaSizes.forEach(size => {
      this.totalPrice[size] = 0;
      this.originalPrices[size] = 0;
      this.finalPrices[size] = 0;
      this.appliedOffers[size] = ''; 
  
      const selectedToppings = this.toppings
        .filter(topping => this.selectionState[topping.name][size])
        .map(topping => topping.name);
  
      if (selectedToppings.length > 0) {
        const pizza: Pizza = {
          size: size,
          toppings: selectedToppings,
          price: 0 
        };
  
        const orderCount = 1; 
        const result = this.pizzaService.checkOffers(pizza, orderCount);
  
        this.totalPrice[size] = result.originalPrice;
        this.originalPrices[size] = result.originalPrice;
        this.finalPrices[size] = result.finalPrice;
        this.appliedOffers[size] = result.appliedOffer ? ` ✔️ ${result.appliedOffer}` : '';
      }
    });
  }
  
  
}
