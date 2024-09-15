import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NewPostComponent } from './new-post.component';
import { NewPostRoutingModule } from './new-post-routing.module';



@NgModule({
  declarations: [
    NewPostComponent
  ],
  imports: [
    CommonModule,
    NewPostRoutingModule
  ]
})
export class NewPostModule { }
