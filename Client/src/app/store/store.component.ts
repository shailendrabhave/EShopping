import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { IProduct } from '../shared/models/IProduct';
import { StoreService } from './store.service';
import { IBrand } from '../shared/models/IBrand';
import { IType } from '../shared/models/IType';
import { StoreParams } from '../shared/models/StoreParams';
import { PageChangedEvent } from 'ngx-bootstrap/pagination';

@Component({
  selector: 'app-store',
  templateUrl: './store.component.html',
  styleUrls: ['./store.component.scss'],
})
export class StoreComponent implements OnInit {
  @ViewChild('search') searchTerm?: ElementRef;
  products: IProduct[] = [];
  brands: IBrand[] = [];
  productTypes: IType[] = [];
  storeParams = new StoreParams();
  totalCount = 0;
  sortOptions = [
    { name: 'Alphabetical', value: 'name' },
    { name: 'Price: Ascending', value: 'priceAsc' },
    { name: 'Price: Descending', value: 'priceDesc' },
  ];

  constructor(private storeService: StoreService) {}

  ngOnInit(): void {
    this.getProducts();

    this.getBrands();

    this.getProductTypes();
  }

  private getProductTypes() {
    this.storeService.getProductTypes().subscribe({
      next: (response) => {
        this.productTypes = [{ id: '', name: 'All' }, ...response];
      },
      error: (error) => console.log(error),
    });
  }

  private getBrands() {
    this.storeService.getBrands().subscribe({
      next: (response) => {
        this.brands = [{ id: '', name: 'All' }, ...response];
      },
      error: (error) => console.log(error),
    });
  }

  private getProducts() {
    this.storeService.getProducts(this.storeParams).subscribe({
      next: (response) => {
        this.products = response.data;
        this.storeParams.pageNumber = response.pageIndex;
        this.storeParams.pageSize = response.pageSize;
        this.totalCount = response.count;
      },
      error: (error) => console.log(error),
    });
  }

  onBrandSelected(brandId: string) {
    this.storeParams.brandId = brandId;
    this.getProducts();
  }

  onProductTypeSelected(productTypeId: string) {
    this.storeParams.typeId = productTypeId;
    this.getProducts();
  }

  onSortSelected(sort: string) {
    this.storeParams.sort = sort;
    this.getProducts();
  }

  onPageChanged(event: PageChangedEvent) {
    this.storeParams.pageNumber = event.page;
    this.getProducts();
  }

  onSearch() {
    this.storeParams.search = this.searchTerm?.nativeElement.value;
    this.storeParams.pageNumber = 1;
    this.getProducts();
  }

  onReset() {
    if (this.searchTerm) this.searchTerm.nativeElement.value = '';

    this.storeParams = new StoreParams();
    this.getProducts();
  }
}
