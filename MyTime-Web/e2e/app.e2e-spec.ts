import { MyTimeWebPage } from './app.po';

describe('my-time-web App', function() {
  let page: MyTimeWebPage;

  beforeEach(() => {
    page = new MyTimeWebPage();
  });

  it('should display message saying app works', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('app works!');
  });
});
