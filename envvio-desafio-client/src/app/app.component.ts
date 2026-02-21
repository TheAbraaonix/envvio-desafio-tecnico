import { Component } from '@angular/core';
import { ROUTE_PATHS } from './app.paths';

@Component({
  selector: 'app-root',
  standalone: false,
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'Sistema de Gerenciamento de Estacionamento';
  routePaths = ROUTE_PATHS;
}
