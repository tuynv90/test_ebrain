// ====================================================
// More Templates: https://www.ebenmonney.com/templates
// Email: support@ebenmonney.com
// ====================================================

import { AppPage } from './app.po';

describe('Superbrain App', () => {
  let page: AppPage;

  beforeEach(() => {
    page = new AppPage();
  });

  it('should display application title: Superbrain', () => {
    page.navigateTo();
    expect(page.getAppTitle()).toEqual('Superbrain');
  });
});
