import { IBasketItem } from './IBasketItem';

export interface IBasket {
  userName: string;
  items: IBasketItem[];
  totalPrice: number;
}
