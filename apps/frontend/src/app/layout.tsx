import { cn } from '@/lib/utils';
import type { Metadata } from 'next';
import { Geist, Geist_Mono } from 'next/font/google';
import { Toaster as Sonner } from 'sonner';
import './globals.css';

// TODO: Update the metadata
export const metadata: Metadata = {
  title: 'Template',
  description: 'A Next.js template with Tailwind CSS and TypeScript',
};

const geistSans = Geist({
  variable: '--font-geist-sans',
  subsets: ['latin'],
});

const geistMono = Geist_Mono({
  variable: '--font-geist-mono',
  subsets: ['latin'],
});

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html lang="en">
      <body
        className={cn(
          geistSans.variable,
          geistMono.variable,
          'antialiased',
          process.env.NODE_ENV === 'development' ? 'debug-screens' : '',
        )}
      >
        {children}
        <Sonner />
      </body>
    </html>
  );
}
