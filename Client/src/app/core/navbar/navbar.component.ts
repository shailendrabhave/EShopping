import { BasketService } from 'src/app/basket/basket.service';
import { Component } from '@angular/core';
import { IBasketItem } from 'src/app/shared/models/IBasketItem';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss'],
})
export class NavbarComponent {
  constructor(public basketService: BasketService) {}

  getBasketCount(basketItems: IBasketItem[]) {
    return basketItems.reduce((sum, item) => (sum += item.quantity), 0);
  }
}
