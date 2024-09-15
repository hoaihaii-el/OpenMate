import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.scss']
})
export class LayoutComponent implements OnInit {
  constructor(private router: Router) { }

  ngOnInit(): void {
    this.isMoblieMenuOpen = false;
    this.activeTab = this.router.url.split('/')[2];
  }
  public isMoblieMenuOpen = false;
  public activeTab = 'home';

  openMobileMenu() {
    this.isMoblieMenuOpen = !this.isMoblieMenuOpen;
  }

  active(destination: string) {
    this.activeTab = destination;
  }
}
