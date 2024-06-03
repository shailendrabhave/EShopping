import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { IBasket } from '../shared/models/IBasket';
import { IProduct } from '../shared/models/IProduct';
import { IBasketItem } from '../shared/models/IBasketItem';

@Injectable({
  providedIn: 'root',
})
export class BasketService {
  baseUrl = 'http://localhost:8010';
  private basketSource = new BehaviorSubject<IBasket | null>(null);
  basketSource$ = this.basketSource.asObservable();
  private basketTotal = new BehaviorSubject<number>(0);
  basketTotal$ = this.basketTotal.asObservable();

  constructor(private httpClient: HttpClient) {}

  getBasket(userName: string) {
    return this.httpClient
      .get<IBasket>(this.baseUrl + '/Basket/GetBasketByUserName/' + userName)
      .subscribe({
        next: (basket) => {
          this.basketSource.next(basket);
          this.calculateBasketTotal();
        },
      });
  }

  setBasket(basket: IBasket) {
    return this.httpClient
      .post<IBasket>(this.baseUrl + '/Basket/CreateBasket', basket)
      .subscribe({
        next: (basket) => {
          this.basketSource.next(basket);
          this.calculateBasketTotal();
        },
      });
  }

  getCurrentBasket() {
    return this.basketSource.value;
  }

  addItemToBasket(item: IProduct, quantity = 1) {
    const itemToAdd = this.mapProductToBasketItem(item);
    const basket = this.getCurrentBasket() ?? this.createBasket();
    basket.items = this.addOrUpdateBasketItems(
      basket.items,
      itemToAdd,
      quantity
    );
    this.setBasket(basket);
  }

  incrementItemQuantity(item: IBasketItem) {
    const basket = this.getCurrentBasket();
    if (!basket) return;

    const itemIndex = basket.items.findIndex(
      (x) => x.productId == item.productId
    );

    if (itemIndex >= 0) basket.items[itemIndex].quantity++;

    this.setBasket(basket);
  }

  decrementItemQuantity(item: IBasketItem) {
    const basket = this.getCurrentBasket();
    if (!basket) return;

    const itemIndex = basket.items.findIndex(
      (x) => x.productId == item.productId
    );

    if (itemIndex >= 0 && basket.items[itemIndex].quantity > 1) {
      basket.items[itemIndex].quantity--;
      this.setBasket(basket);
    } else this.removeItemFromBasket(item);
  }

  removeItemFromBasket(item: IBasketItem) {
    const basket = this.getCurrentBasket();
    if (!basket) return;

    basket.items = basket.items.filter((x) => x.productId !== item.productId);

    if (basket.items.length > 0) this.setBasket(basket);
    else this.deleteBasket(basket.userName);
  }

  private deleteBasket(userName: string) {
    return this.httpClient
      .delete(this.baseUrl + '/Basket/DeleteBasketByUserName/' + userName)
      .subscribe({
        next: (response) => {
          this.basketSource.next(null);
          this.basketTotal.next(0);
          localStorage.removeItem('basket_username');
        },
        error: (error) =>
          console.log('Error occured while deleting basket', error),
      });
  }

  private addOrUpdateBasketItems(
    items: IBasketItem[],
    itemToAdd: IBasketItem,
    quantity: number
  ): IBasketItem[] {
    const item = items.find((x) => x.productId == itemToAdd.productId);

    if (item) {
      item.quantity += quantity;
    } else {
      itemToAdd.quantity = quantity;
      items.push(itemToAdd);
    }

    return items;
  }

  private createBasket(): IBasket {
    const basket: IBasket = {
      userName: 'shailendra.b',
      totalPrice: 0,
      items: [],
    };
    localStorage.setItem('basket_username', 'shailendra.b');
    return basket;
  }

  private mapProductToBasketItem(item: IProduct): IBasketItem {
    return {
      productId: item.id,
      price: item.price,
      productImageFile: item.imageFile,
      productName: item.summary,
      quantity: 0,
    };
  }

  private calculateBasketTotal() {
    const basket = this.getCurrentBasket();
    if (!basket) return;

    var totalCost = basket.items.reduce(
      (totalCost, item) => item.price * item.quantity + totalCost,
      0
    );

    this, this.basketTotal.next(totalCost);
  }
}
