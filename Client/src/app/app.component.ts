import { Component, OnInit } from '@angular/core';
import { BasketService } from './basket/basket.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnInit {
  title = 'Welcome to EShopping';

  constructor(private basketService: BasketService) {}

  ngOnInit(): void {
    const basketUserName = localStorage.getItem('basket_username');
    if (basketUserName) {
      this.basketService.getBasket(basketUserName);
    }
  }
}
