import org.jetbrains.kotlin.gradle.tasks.KotlinCompile

plugins {
    kotlin("jvm") version "2.1.0"
    application
}

group = "me.scmacdon"
version = "1.0-SNAPSHOT"

java {
    sourceCompatibility = JavaVersion.VERSION_17
    targetCompatibility = JavaVersion.VERSION_17
}

buildscript {
    repositories {
        maven("https://plugins.gradle.org/m2/")
    }
    dependencies {
        classpath("org.jlleitschuh.gradle:ktlint-gradle:12.1.1")
    }
}

repositories {
    mavenCentral()
}
apply(plugin = "org.jlleitschuh.gradle.ktlint")
dependencies {
    implementation(platform("aws.sdk.kotlin:bom:1.4.119"))
    implementation("aws.sdk.kotlin:ecr")
    implementation("aws.sdk.kotlin:secretsmanager")
    implementation("aws.smithy.kotlin:http-client-engine-okhttp")
    implementation("aws.smithy.kotlin:http-client-engine-crt")
    implementation("com.google.code.gson:gson:2.10")
    testImplementation("org.junit.jupiter:junit-jupiter:5.9.2")
    implementation("org.jetbrains.kotlinx:kotlinx-coroutines-core:1.6.4")
    implementation("org.slf4j:slf4j-api:1.7.36") // Updated SLF4J version
    implementation("com.github.docker-java:docker-java-core:3.3.6")
    implementation("com.github.docker-java:docker-java-transport-httpclient5:3.3.6")
    implementation("com.github.docker-java:docker-java:3.3.6")
    implementation("org.slf4j:slf4j-api:2.0.15")
    implementation("org.slf4j:slf4j-simple:2.0.15")
}
tasks.withType<KotlinCompile> {
    kotlinOptions.jvmTarget = "17"
}

tasks.test {
    useJUnitPlatform()
    testLogging {
        events("passed", "skipped", "failed")
    }

    // Define the test source set
    testClassesDirs += files("build/classes/kotlin/test")
    classpath += files("build/classes/kotlin/main", "build/resources/main")
}
