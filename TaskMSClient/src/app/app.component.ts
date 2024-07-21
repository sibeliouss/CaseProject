import { Component, Renderer2 } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NavbarComponent } from "./shared/navbar/navbar.component";
import { CommonModule } from '@angular/common';
import { ProgressSpinnerModule } from 'primeng/progressspinner';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, NavbarComponent, CommonModule, ProgressSpinnerModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'TaskMSClient';
  isLoading = true;

  constructor(private renderer: Renderer2) {}

  ngOnInit() {
    // Yükleme durumunu simüle et
    setTimeout(() => {
      this.isLoading = false;
      this.renderer.removeClass(document.body, 'loading');
    }, 3000);

    // Yükleme başladığında body'ye loading sınıfı ekle
    this.renderer.addClass(document.body, 'loading');
  }
}
