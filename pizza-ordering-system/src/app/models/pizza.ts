export interface Pizza {
    size: string;
    toppings: string[];
    price: number;
}
  
  export interface Topping {
    selected?: boolean;
    name: string;
    price: number;
    type: 'veg' | 'non-veg';
  }
 
  export type PizzaSize = 'small' | 'medium' | 'large' | 'extraLarge';

  export const OFFERS = {
    OFFER1: {
      description: "1 Medium Pizza with 2 toppings = $5",
      price: 5,
      requiredSize: 'medium',
      requiredToppings: 2
    },
    OFFER2: {
      description: "2 Medium Pizzas with 4 toppings each = $9",
      price: 9,
      requiredSize: 'medium',
      requiredToppings: 4,
      pizzaCount: 2
    },
    OFFER3: {
      description: "1 Large Pizza with 4 toppings (Pepperoni and Barbecue Chicken count as 2 toppings) - 50% discount",
      discount: 0.5,
      requiredSize: 'large',
      requiredToppings: 4,
      specialToppings: ['Pepperoni', 'Barbecue chicken']
    }
  };
  
  export const SIZES: Record<PizzaSize, number> = {
    small: 5,
    medium: 7,
    large: 8,
    extraLarge: 9
  };
  export const TOPPINGS: Topping[] = [
    { name: 'Tomatoes', price: 1.00, type: 'veg' },
    { name: 'Onions', price: 0.50, type: 'veg' },
    { name: 'Bell pepper', price: 1.00, type: 'veg' },
    { name: 'Mushrooms', price: 1.20, type: 'veg' },
    { name: 'Pineapple', price: 0.75, type: 'veg' },
    { name: 'Sausage', price: 1.00, type: 'non-veg' },
    { name: 'Pepperoni', price: 2.00, type: 'non-veg' },
    { name: 'Barbecue chicken', price: 3.00, type: 'non-veg' }
  ];
