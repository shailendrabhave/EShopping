import { Component, OnInit } from '@angular/core';
import { StoreService } from '../store.service';
import { IProduct } from 'src/app/shared/models/IProduct';
import { ActivatedRoute } from '@angular/router';
import { BreadcrumbService } from 'xng-breadcrumb';
import { BasketService } from 'src/app/basket/basket.service';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.scss'],
})
export class ProductDetailsComponent implements OnInit {
  product?: IProduct;
  quantity = 0;

  constructor(
    private storeService: StoreService,
    private activatedRoute: ActivatedRoute,
    private breadcrumbService: BreadcrumbService,
    private basketService: BasketService
  ) {}

  ngOnInit(): void {
    this.loadProductDetails();
  }

  private loadProductDetails() {
    const productId = this.activatedRoute.snapshot.paramMap.get('id');
    if (productId) {
      this.storeService.getProductById(productId).subscribe({
        next: (response) => {
          this.product = response;
          this.breadcrumbService.set('@productDetails', response.summary);
        },
        error: (error) => console.log(error),
      });
    }
  }

  incrementQuantity() {
    this.quantity++;
  }

  decrementQuantity() {
    if (this.quantity > 0) this.quantity--;
  }

  addItemToCart() {
    if (this.product)
      this.basketService.addItemToBasket(this.product, this.quantity);
  }
}
