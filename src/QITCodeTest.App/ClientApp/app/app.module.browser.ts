import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppModuleShared } from './app.module.shared';
import { AppComponent } from './app.component';

@NgModule({
    bootstrap: [AppComponent],
    imports: [
        BrowserModule,
        AppModuleShared
    ]
})
export class AppModule {
}

