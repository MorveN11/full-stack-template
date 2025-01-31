import type { Metadata } from 'next';
import { Toaster as Sonner } from 'sonner';
import './globals.css';

// TODO: Update the metadata
export const metadata: Metadata = {
  title: 'Template',
  description: 'A Next.js template with Tailwind CSS and TypeScript',
};

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html lang="en" suppressHydrationWarning>
      <body className="min-h-screen flex bg-background font-sans antialiased">
        {children}
        <Sonner />
      </body>
    </html>
  );
}
