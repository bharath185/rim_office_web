//package com.officeconnect.config;
//
//import org.springframework.context.annotation.Bean;
//import org.springframework.context.annotation.Configuration;
//import org.springframework.web.servlet.config.annotation.CorsRegistry;
//import org.springframework.web.servlet.config.annotation.ResourceHandlerRegistry;
//import org.springframework.web.servlet.config.annotation.WebMvcConfigurer;
//
//import java.nio.file.Paths;
//
//@Configuration
//public class AppConfig {
//
//    @Bean
//    public WebMvcConfigurer corsConfigurer() {
//        return new WebMvcConfigurer() {
//            @Override
//            public void addCorsMappings(CorsRegistry registry) {
//                registry.addMapping("/**")
//                        .allowedOrigins("*")
//                        .allowedMethods("GET", "POST", "PUT", "DELETE", "OPTIONS")
//                        .allowedHeaders("*")
//                        .allowCredentials(false)
//                        .maxAge(3600);
//            }
//
//            @Override
//            public void addResourceHandlers(ResourceHandlerRegistry registry) {
//                String uploadsPath = Paths.get("Uploads").toAbsolutePath().toUri().toString();
//                registry.addResourceHandler("/Uploads/**")
//                        .addResourceLocations(uploadsPath);
//            }
//        };
//    }
//}


package com.officeconnect.config;

import org.springframework.beans.factory.annotation.Value;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.web.servlet.config.annotation.CorsRegistry;
import org.springframework.web.servlet.config.annotation.ResourceHandlerRegistry;
import org.springframework.web.servlet.config.annotation.WebMvcConfigurer;

import java.nio.file.Paths;

@Configuration
public class AppConfig {

    // Optionally override the uploads base directory via application.properties:
    //   app.uploads.dir=C:/java/Uploads
    // Defaults to a relative "Uploads" folder next to the running jar.
    @Value("${app.uploads.dir:Uploads}")
    private String uploadsDir;

    @Bean
    public WebMvcConfigurer corsConfigurer() {
        return new WebMvcConfigurer() {
            @Override
            public void addCorsMappings(CorsRegistry registry) {
                registry.addMapping("/**")
                        .allowedOrigins("*")
                        .allowedMethods("GET", "POST", "PUT", "DELETE", "OPTIONS")
                        .allowedHeaders("*")
                        .allowCredentials(false)
                        .maxAge(3600);
            }

            @Override
            public void addResourceHandlers(ResourceHandlerRegistry registry) {
                // Resolve the uploads directory to an absolute URI.
                // Spring requires the location URI to end with "/" so that
                // sub-path resolution works correctly (e.g. File/Govt/Pancard/x.pdf).
                String uploadsPath = Paths.get(uploadsDir).toAbsolutePath().toUri().toString();
                if (!uploadsPath.endsWith("/")) {
                    uploadsPath = uploadsPath + "/";
                }
                registry.addResourceHandler("/Uploads/**")
                        .addResourceLocations(uploadsPath);
            }
        };
    }
}