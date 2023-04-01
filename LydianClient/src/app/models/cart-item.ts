import { Product } from './product';

export class CartItem{
  public cartItemId:number = 0;
  public cartId:number = 0;
  public productId:number = 0;
  public product:Product = new Product();
  public quantity:number = 0;
  public updatedAt:Date = new Date();

}

