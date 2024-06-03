import { Component } from '@angular/core';
import { BasketService } from './basket.service';
import { IBasketItem } from '../shared/models/IBasketItem';

@Component({
  selector: 'app-basket',
  templateUrl: './basket.component.html',
  styleUrls: ['./basket.component.scss'],
})
export class BasketComponent {
  constructor(public basketService: BasketService) {}

  decrementItem(basketItem: IBasketItem) {
    this.basketService.decrementItemQuantity(basketItem);
  }

  incrementItem(basketItem: IBasketItem) {
    this.basketService.incrementItemQuantity(basketItem);
  }

  removeBasketItem(basketItem: IBasketItem) {
    this.basketService.removeItemFromBasket(basketItem);
  }
}
