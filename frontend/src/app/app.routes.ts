import { Routes } from '@angular/router';

export const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'produtos' },
  {
    path: 'produtos',
    loadComponent: () =>
      import('./features/produtos/produtos.component').then((m) => m.ProdutosComponent)
  }
];
