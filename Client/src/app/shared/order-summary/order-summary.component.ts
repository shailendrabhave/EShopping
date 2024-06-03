import { BasketService } from 'src/app/basket/basket.service';
import { Component } from '@angular/core';

@Component({
  selector: 'app-order-summary',
  templateUrl: './order-summary.component.html',
  styleUrls: ['./order-summary.component.scss'],
})
export class OrderSummaryComponent {
  constructor(public basketService: BasketService) {}
}
