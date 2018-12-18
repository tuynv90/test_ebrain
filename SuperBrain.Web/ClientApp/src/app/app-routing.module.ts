// ====================================================
// More Templates: https://www.ebenmonney.com/templates
// Email: support@ebenmonney.com
// ====================================================

import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { LoginComponent } from "./components/login/login.component";
import { HomeComponent } from "./components/home/home.component";

import { NotFoundComponent } from "./components/not-found/not-found.component";
import { AuthService } from './services/auth.service';
import { AuthGuard } from './services/auth-guard.service';

const routes: Routes = [
  //{ path: '', redirectTo: 'home', pathMatch: 'full' },
  { path: "", component: HomeComponent, canActivate: [AuthGuard], data: { title: "Home" } },
  { path: "login", component: LoginComponent, data: { title: "Login" } },
  { path: "home", redirectTo: "/", pathMatch: "full" },
  { path: "**", component: NotFoundComponent, data: { title: "Page Not Found" } },
  { path: 'lazymodule', loadChildren: './lazymodule/lazymodule.module#LazyModuleModule' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
  providers: [AuthService, AuthGuard]
})

export class AppRoutingModule { }
