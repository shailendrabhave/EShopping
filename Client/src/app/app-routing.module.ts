import { HomeComponent } from './home/home.component';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { NotFoundComponent } from './core/not-found/not-found.component';
import { UnauthorizedComponent } from './core/unauthorized/unauthorized.component';
import { ServerErrorComponent } from './core/server-error/server-error.component';

const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'not-found', component: NotFoundComponent },
  { path: 'unauthorized', component: UnauthorizedComponent },
  { path: 'server-error', component: ServerErrorComponent },
  {
    path: 'store',
    loadChildren: () =>
      import('./store/store.module').then((module) => module.StoreModule),
    data: { breadcrumb: 'Store' },
  },
  {
    path: 'basket',
    loadChildren: () =>
      import('./basket/basket.module').then((module) => module.BasketModule),
    data: { breadcrumb: 'Basket' },
  },
  {
    path: 'account',
    loadChildren: () =>
      import('./account/account.module').then((module) => module.AccountModule),
    data: { breadcrumb: 'Account' },
  },
  { path: '**', redirectTo: '', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
