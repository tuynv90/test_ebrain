import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { TestComponent } from './test/test.component';

const routes: Routes = [
  { path: '', component: TestComponent },
];

@NgModule({
  imports: [
    RouterModule.forChild(routes)
  ],
  declarations: [TestComponent]
})

export class LazyModuleModule { }
