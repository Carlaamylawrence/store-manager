import { sveltekit } from '@sveltejs/kit/vite';
import path from 'path';
import { defineConfig } from 'vite';

export default defineConfig({
	plugins: [sveltekit()],
  resolve: {
    alias: {
      '@components': path.resolve('./src/components'),
    }
  },
	  server: {
    proxy: {
      '/api': {
        target: 'http://localhost:5226',
        changeOrigin: true,
        rewrite: (path) => path.replace(/^\/api/, '')
      }
    }
  }
});
