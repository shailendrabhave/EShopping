import { BasketService } from 'src/app/basket/basket.service';
import { Component, Input } from '@angular/core';
import { IProduct } from 'src/app/shared/models/IProduct';

@Component({
  selector: 'app-product-items',
  templateUrl: './product-items.component.html',
  styleUrls: ['./product-items.component.scss'],
})
export class ProductItemsComponent {
  @Input() Product?: IProduct;

  constructor(private basketService: BasketService) {}

  addToBasket() {
    this.Product && this.basketService.addItemToBasket(this.Product);
  }
}
