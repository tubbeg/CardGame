import { createRoot } from 'react-dom/client';

function NavigationBar() {
  // TODO: Actually implement a navigation bar
  return <h1>Hello from React!</h1>;
}
const myElement = <h1>Hello, world!</h1>;

function initReact(){

  const domNode = document.getElementById('root');
  const root = createRoot(domNode);
  root.render(<NavigationBar />);
  
}

export {initReact}